namespace Order.Application.EventHandler;

public interface IEventHandler<in TEvent> where TEvent : class
{
    Task HandleAsync(TEvent domainEvent);
}