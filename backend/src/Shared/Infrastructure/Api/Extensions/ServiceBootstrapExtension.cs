using Order.Infrastructure.IOC;

namespace Api.Extensions;

public static class ServiceBootstrapExtension
{
    public static IServiceCollection Bootstrap(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        // Register validation
        services.AddValidation();

        // Register database
        services.AddDatabase(configuration);

        // Register repositories
        services.AddRepositories();

        // Register use cases
        services.AddUseCases();

        // Register Azure Service Bus
        services.AddServiceBus(configuration);

        // Register event infrastructure
        services.AddEvents();

        // Register health checks
        services.AddApplicationHealthChecks(configuration);
        
        return services;
    }
}