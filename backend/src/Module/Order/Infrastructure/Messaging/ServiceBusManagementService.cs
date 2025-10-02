using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace Order.Infrastructure.Messaging;

public interface IServiceBusManagementService
{
    Task<QueueStats> GetQueueStatsAsync(string queueName);
    Task<List<ServiceBusReceivedMessage>> PeekMessagesAsync(string queueName, int maxMessages = 10);
    Task<int> PurgeQueueAsync(string queueName);
}

public class ServiceBusManagementService : IServiceBusManagementService
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusAdministrationClient _adminClient;

    public ServiceBusManagementService(string connectionString)
    {
        _client = new ServiceBusClient(connectionString);
        _adminClient = new ServiceBusAdministrationClient(connectionString);
    }

    public async Task<QueueStats> GetQueueStatsAsync(string queueName)
    {
        var queueRuntimeProperties = await _adminClient.GetQueueRuntimePropertiesAsync(queueName);

        return new QueueStats
        {
            QueueName = queueName,
            ActiveMessageCount = queueRuntimeProperties.Value.ActiveMessageCount,
            DeadLetterMessageCount = queueRuntimeProperties.Value.DeadLetterMessageCount,
            ScheduledMessageCount = queueRuntimeProperties.Value.ScheduledMessageCount,
            TotalMessageCount = queueRuntimeProperties.Value.TotalMessageCount,
            SizeInBytes = queueRuntimeProperties.Value.SizeInBytes
        };
    }

    public async Task<List<ServiceBusReceivedMessage>> PeekMessagesAsync(string queueName, int maxMessages = 10)
    {
        var receiver = _client.CreateReceiver(queueName, new ServiceBusReceiverOptions
        {
            ReceiveMode = ServiceBusReceiveMode.PeekLock
        });

        var messages = await receiver.PeekMessagesAsync(maxMessages);
        await receiver.DisposeAsync();

        return messages.ToList();
    }

    public async Task<int> PurgeQueueAsync(string queueName)
    {
        var receiver = _client.CreateReceiver(queueName);
        int count = 0;

        try
        {
            while (true)
            {
                var messages = await receiver.ReceiveMessagesAsync(100, TimeSpan.FromSeconds(1));

                if (messages == null || !messages.Any())
                    break;

                foreach (var message in messages)
                {
                    await receiver.CompleteMessageAsync(message);
                    count++;
                }
            }
        }
        finally
        {
            await receiver.DisposeAsync();
        }

        return count;
    }
}

public class QueueStats
{
    public string QueueName { get; set; } = string.Empty;
    public long ActiveMessageCount { get; set; }
    public long DeadLetterMessageCount { get; set; }
    public long ScheduledMessageCount { get; set; }
    public long TotalMessageCount { get; set; }
    public long SizeInBytes { get; set; }
}