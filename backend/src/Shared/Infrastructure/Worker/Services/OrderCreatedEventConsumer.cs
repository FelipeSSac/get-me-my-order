using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Order.Domain.Event;

namespace Worker.Services;

public class OrderCreatedEventConsumer : BackgroundService
{
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly ServiceBusProcessor _processor;
    private readonly ServiceBusClient _client;

    public OrderCreatedEventConsumer(
        ILogger<OrderCreatedEventConsumer> logger,
        IConfiguration configuration)
    {
        _logger = logger;

        var connectionString = configuration["SERVICEBUS_CONNECTION_STRING"]
            ?? throw new InvalidOperationException("Service Bus connection string is not configured");

        var queueName = configuration["SERVICEBUS_QUEUE_NAME"]
            ?? throw new InvalidOperationException("Service Bus queue name is not configured");

        _client = new ServiceBusClient(connectionString);
        _processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1
        });

        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OrderCreatedEventConsumer is starting...");

        await _processor.StartProcessingAsync(stoppingToken);

        _logger.LogInformation("OrderCreatedEventConsumer is now listening for messages...");

        // Keep the service running
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var body = args.Message.Body.ToString();
            _logger.LogInformation("Received message: {Body}", body);

            var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(body);

            if (orderCreatedEvent != null)
            {
                _logger.LogInformation(
                    "Processing OrderCreated event - OrderId: {OrderId}, ClientId: {ClientId}, TotalValue: {TotalValue} {Currency}",
                    orderCreatedEvent.OrderId,
                    orderCreatedEvent.ClientId,
                    orderCreatedEvent.TotalValue,
                    orderCreatedEvent.Currency
                );

                // TODO: Add your business logic here (e.g., send email, notify external systems, etc.)
                await ProcessOrderCreatedAsync(orderCreatedEvent);

                // Complete the message so it's removed from the queue
                await args.CompleteMessageAsync(args.Message);
                _logger.LogInformation("Message completed successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            // Don't complete the message - it will be retried
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private Task ProcessOrderCreatedAsync(OrderCreatedEvent orderCreatedEvent)
    {
        // Add your business logic here
        // Examples:
        // - Send confirmation email to customer
        // - Notify warehouse system
        // - Update inventory
        // - Send notifications to external systems

        _logger.LogInformation("Order {OrderId} has been created successfully!", orderCreatedEvent.OrderId);

        return Task.CompletedTask;
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error in Service Bus processor: {ErrorSource}", args.ErrorSource);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("OrderCreatedEventConsumer is stopping...");

        await _processor.StopProcessingAsync(cancellationToken);
        await _processor.DisposeAsync();
        await _client.DisposeAsync();

        await base.StopAsync(cancellationToken);
    }
}
