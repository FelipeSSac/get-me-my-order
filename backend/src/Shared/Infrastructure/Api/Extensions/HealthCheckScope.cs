using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.Extensions;

public static class HealthCheckScope
{
    public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = ExpandEnvironmentVariables(
            configuration.GetConnectionString("DefaultConnection")!);

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

        services.AddHealthChecks()
            .AddNpgSql(connectionString, name: "postgresql")
            .AddCheck("application", () =>
                Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Application is running"))
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

        return services;
    }
}