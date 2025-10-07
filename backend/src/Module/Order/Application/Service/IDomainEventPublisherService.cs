namespace Order.Application.Service;

public interface IDomainEventPublisherService
{
    Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : class;
}