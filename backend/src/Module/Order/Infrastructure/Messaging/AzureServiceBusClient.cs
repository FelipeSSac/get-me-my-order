using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace Order.Infrastructure.Messaging;

public class AzureServiceBusClient : IServiceBusClient, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly Dictionary<string, ServiceBusSender> _senders;

    public AzureServiceBusClient(string connectionString)
    {
        _client = new ServiceBusClient(connectionString);
        _senders = new Dictionary<string, ServiceBusSender>();
    }

    public async Task SendMessageAsync<T>(T message, string queueName) where T : class
    {
        var sender = GetOrCreateSender(queueName);

        var jsonMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(jsonMessage)
        {
            ContentType = "application/json",
            Subject = typeof(T).Name
        };

        await sender.SendMessageAsync(serviceBusMessage);
    }

    private ServiceBusSender GetOrCreateSender(string queueName)
    {
        if (!_senders.ContainsKey(queueName))
        {
            _senders[queueName] = _client.CreateSender(queueName);
        }

        return _senders[queueName];
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var sender in _senders.Values)
        {
            await sender.DisposeAsync();
        }

        await _client.DisposeAsync();
    }
}