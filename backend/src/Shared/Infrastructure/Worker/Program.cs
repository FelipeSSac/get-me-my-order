using Worker.Services;
using DotNetEnv;

// Load environment variables from .env file relative to project root
Env.Load("../../../../.env");

var builder = Host.CreateApplicationBuilder(args);

// Configure to use environment variables in configuration
builder.Configuration.AddEnvironmentVariables();

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
