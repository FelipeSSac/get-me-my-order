using Order.Infrastructure.IOC;

namespace Worker.Extensions;

public static class ServiceBootstrapExtension
{
    public static IServiceCollection Bootstrap(this IServiceCollection services, IConfiguration configuration)
    {
        // Register database
        services.AddDatabase(configuration);

        // Register Azure Service Bus
        services.AddServiceBus(configuration);

        // Register events
        services.AddEvents();

        // Register SignalR
        services.AddSignalRConfiguration();

        // Register CORS
        services.AddCorsConfiguration(configuration);

        // Register worker services
        services.AddWorkerServices();

        return services;
    }
}