namespace Order.Infrastructure.Messaging;

public interface IServiceBusClient
{
    Task SendMessageAsync<T>(T message, string queueName) where T : class;
}