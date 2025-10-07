using Order.Application.Service;
using Order.Application.UseCase;
using Order.Application.UseCase.Interface;
using Order.Domain.Repository;
using Order.Infrastructure.Persistence.EntityFramework.Repository;
using Worker.Services;

namespace Worker.Extensions;

public static class WorkerServicesScope
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped<IOrderRepository, OrderRepositoryEntityFramework>();

        // Register use cases
        services.AddScoped<IProcessOrderUseCase, ProcessOrderUseCase>();

        // Register notification service
        services.AddSingleton<IOrderNotificationService, OrderNotificationService>();

        // Register the OrderCreatedEventConsumer as a hosted service
        services.AddHostedService<OrderCreatedEventConsumer>();

        return services;
    }
}
