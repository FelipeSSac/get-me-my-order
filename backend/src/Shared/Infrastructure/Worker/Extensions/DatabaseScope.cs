using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Data;

namespace Worker.Extensions;

public static class DatabaseScope
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = ConfigurationHelper.ExpandEnvironmentVariables(
            configuration.GetConnectionString("DefaultConnection")!);

        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}