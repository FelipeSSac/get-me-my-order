namespace Order.Application.EventHandler.Interface;

public interface IEventHandler<in TEvent> where TEvent : class
{
    Task HandleAsync(TEvent domainEvent);
}