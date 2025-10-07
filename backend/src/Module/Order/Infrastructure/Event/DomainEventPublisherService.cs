using Order.Application.EventHandler.Interface;
using Order.Application.Service;

namespace Order.Infrastructure.Event;

public class DomainEventPublisherService : IDomainEventPublisherService
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisherService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : class
    {
        var handlers = _serviceProvider.GetService(typeof(IEnumerable<IEventHandler<TEvent>>))
                       as IEnumerable<IEventHandler<TEvent>>;

        if (handlers == null || !handlers.Any())
            return;

        var tasks = handlers.Select(handler => handler.HandleAsync(domainEvent));
        await Task.WhenAll(tasks);
    }
}