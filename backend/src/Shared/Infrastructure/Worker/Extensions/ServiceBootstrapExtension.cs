namespace Worker.Extensions;

public static class ServiceBootstrapExtension
{
    public static IServiceCollection Bootstrap(this IServiceCollection services, IConfiguration configuration)
    {
        // Register database
        services.AddDatabase(configuration);

        // Register SignalR
        services.AddSignalRConfiguration();

        // Register CORS
        services.AddCorsConfiguration(configuration);

        // Register worker services
        services.AddWorkerServices();

        return services;
    }
}