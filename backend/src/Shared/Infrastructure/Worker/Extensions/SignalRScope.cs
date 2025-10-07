using Worker.Hubs;

namespace Worker.Extensions;

public static class SignalRScope
{
    public static IServiceCollection AddSignalRConfiguration(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }

    public static WebApplication MapSignalRHubs(this WebApplication app)
    {
        app.MapHub<OrderHub>("/hubs/orders");
        return app;
    }
}
