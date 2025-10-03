using Worker.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;
using Order.Domain.Repository;
using Order.Infrastructure.Persistence.EntityFramework.Repository;
using Order.Application.UseCase;
using Order.Application.UseCase.Interface;
using System.Text.RegularExpressions;

// Load environment variables from .env file relative to project root
Env.Load("../../../../.env");

var builder = Host.CreateApplicationBuilder(args);

// Configure to use environment variables in configuration
builder.Configuration.AddEnvironmentVariables();

// Register database
var connectionString = ExpandEnvironmentVariables(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IOrderRepository, OrderRepositoryEntityFramework>();

// Register use cases
builder.Services.AddScoped<IProcessOrderUseCase, ProcessOrderUseCase>();

// Register the OrderCreatedEventConsumer as a hosted service
builder.Services.AddHostedService<OrderCreatedEventConsumer>();

var host = builder.Build();
host.Run();

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
