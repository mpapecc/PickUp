using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class IdempotentAttribute : Attribute, IAsyncActionFilter
{
    private const int DefaultCacheTimeInMinutes = 60;
    private readonly TimeSpan _cacheDuration;
    public static readonly string key = "Idempotence-Key";
    public IdempotentAttribute(int cacheTimeInMinutes = DefaultCacheTimeInMinutes)
    {
        _cacheDuration = TimeSpan.FromMinutes(cacheTimeInMinutes);
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        // Parse the Idempotence-Key header from the request
        if (!context.HttpContext.Request.Headers.TryGetValue(
                key,
                out StringValues idempotenceKeyValue) ||
            !Guid.TryParse(idempotenceKeyValue, out Guid idempotenceKey))
        {
            context.Result = new BadRequestObjectResult("Invalid or missing Idempotence-Key header");
            return;
        }

        IDistributedCache cache = context.HttpContext
            .RequestServices.GetRequiredService<IDistributedCache>();

        // Check if we already processed this request and return a cached response (if it exists)
        string cacheKey = $"Idempotent_{idempotenceKey}";
        string? cachedResult = await cache.GetStringAsync(cacheKey);
        if (cachedResult is not null)
        {
            IdempotentResponse response = JsonSerializer.Deserialize<IdempotentResponse>(cachedResult, options: JsonSerializerOptions.Web)!;

            var result = new ObjectResult(response.Value) { StatusCode = response.StatusCode };
            context.Result = result;

            return;
        }

        ActionExecutedContext executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult)
        {
            int statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
            IdempotentResponse response = new(statusCode, objectResult.Value);

            await cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(response, options: JsonSerializerOptions.Web),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheDuration }
            );
        }
    }
}

internal sealed class IdempotentResponse
{
    [JsonConstructor]
    public IdempotentResponse(int statusCode, object? value)
    {
        StatusCode = statusCode;
        Value = value;
    }

    public int StatusCode { get; }
    public object? Value { get; }
}