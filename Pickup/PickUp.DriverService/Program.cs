using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PickUp.Common.Application;
using PickUp.Common.Infrastructure.Database;
using PickUp.Common.Infrastructure.Persistance;
using PickUp.DriverService.Infrastructure.Persistance;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<BaseDbContext, DriverServiceDbContext>();

builder.Services.AddDbContext<DriverServiceDbContext>(o => {
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), o =>
    {
        o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "driverservice");
        o.UseNetTopologySuite();
    });
});

builder.Services.AddDistributedPostgresCache(options => {
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = builder.Configuration.GetValue<string>("PostgresCache:SchemaName");
    options.TableName = builder.Configuration.GetValue<string>("PostgresCache:TableName");
    options.CreateIfNotExists = builder.Configuration.GetValue<bool>("PostgresCache:CreateIfNotExists");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
