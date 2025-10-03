using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.Messaging;

namespace Order.Infrastructure.IOC;

public static class ServiceBusIoC
{
    public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceBusConnectionString = configuration["SERVICEBUS_CONNECTION_STRING"]
                                         ?? throw new InvalidOperationException("Service Bus connection string is not configured");
        
        services.AddSingleton<IServiceBusClient>(_ => new AzureServiceBusClient(serviceBusConnectionString));
        services.AddSingleton<IServiceBusManagementService>(_ =>
            new ServiceBusManagementService(serviceBusConnectionString));

        return services;
    }
}