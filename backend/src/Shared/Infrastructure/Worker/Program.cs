using Worker;
using DotNetEnv;
using System.Text.RegularExpressions;

// Load environment variables from .env file
Env.Load();

var builder = Host.CreateApplicationBuilder(args);

// Configure to use environment variables in configuration
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddHostedService<Worker.Worker>();

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
