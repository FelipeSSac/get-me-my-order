using Microsoft.Extensions.DependencyInjection;
using Order.Application.EventHandler;
using Order.Application.EventHandler.Interface;
using Order.Application.Service;
using Order.Domain.Event;
using Order.Infrastructure.Event;

namespace Order.Infrastructure.IOC;

public static class EventIoC
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventPublisherService, DomainEventPublisherService>();
        services.AddScoped<IEventHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();

        return services;
    }
}