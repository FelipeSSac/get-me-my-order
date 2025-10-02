using Order.Application.EventHandler;
using Order.Domain.Event;

namespace Order.Infrastructure.Event;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisher(IServiceProvider serviceProvider)
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