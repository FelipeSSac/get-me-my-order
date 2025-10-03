using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.Data;

namespace Api.Extensions;

public static class DatabaseScope
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
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

        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}