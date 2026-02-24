using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PickUp.Common.Application;
using PickUp.Common.Infrastructure.Database;
using PickUp.Common.Infrastructure.Persistance;
using PickUp.TripService.Application;
using PickUp.TripService.Infrastructure;
using PickUp.TripService.Infrastructure.Persistance;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<BaseDbContext, TripServiceDbContext>();

builder.Services.AddDbContext<TripServiceDbContext>(o => {
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), o =>
    {
        o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "tripservice");
    });
});

builder.Services.AddDistributedPostgresCache(options => {
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = builder.Configuration.GetValue<string>("PostgresCache:SchemaName");
    options.TableName = builder.Configuration.GetValue<string>("PostgresCache:TableName");
    options.CreateIfNotExists = builder.Configuration.GetValue<bool>("PostgresCache:CreateIfNotExists");
});

builder.Services.AddHttpClient<IDriverService, DriverService>(
    client =>
    {
        // Set the base address of the typed client.
        client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("DriverService"));

        // Add a user-agent default request header.
        //client.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-docs");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<TripServiceDbContext>();
        await db.Database.ExecuteSqlRawAsync("CREATE EXTENSION IF NOT EXISTS postgis;");
        await db.Database.MigrateAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
