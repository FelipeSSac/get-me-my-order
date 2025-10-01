using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;
using DotNetEnv;
using System.Text.RegularExpressions;
using Order.Application.UseCase;
using Order.Application.UseCase.Interface;
using Order.Domain.Repository;
using Order.Infrastructure.Persistence.EntityFramework.Repository;

Env.Load("../../../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = ExpandEnvironmentVariables(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IOrderRepository, OrderRepositoryEntityFramework>();
builder.Services.AddScoped<IClientRepository, ClientRepositoryEntityFramework>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryEntityFramework>();

// Register use cases
builder.Services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "postgresql")
    .AddCheck("application", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Application is running"))
    .AddCheck("memory", () =>
    {
        var allocated = GC.GetTotalMemory(forceFullCollection: false);
        var data = new Dictionary<string, object>()
        {
            { "Allocated", allocated },
            { "Gen0Collections", GC.CollectionCount(0) },
            { "Gen1Collections", GC.CollectionCount(1) },
            { "Gen2Collections", GC.CollectionCount(2) }
        };
        return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Memory usage is normal", data);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false,
    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

// Helper function to expand environment variables in configuration strings
static string ExpandEnvironmentVariables(string input)
{
    if (string.IsNullOrEmpty(input))
        return input;

    return Regex.Replace(input, @"\$\{([^}]+)\}", match =>
    {
        var envVarName = match.Groups[1].Value;
        return Environment.GetEnvironmentVariable(envVarName) ?? match.Value;
    });
}
