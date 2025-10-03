using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Order.Domain.Event;
using Order.Application.UseCase.Interface;

namespace Worker.Services;

public class OrderCreatedEventConsumer : BackgroundService
{
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly ServiceBusProcessor _processor;
    private readonly ServiceBusClient _client;
    private readonly IServiceProvider _serviceProvider;

    public OrderCreatedEventConsumer(
        ILogger<OrderCreatedEventConsumer> logger,
        IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

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

                await ProcessOrderCreatedAsync(orderCreatedEvent);

                await args.CompleteMessageAsync(args.Message);
                _logger.LogInformation("Message completed successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private async Task ProcessOrderCreatedAsync(OrderCreatedEvent orderCreatedEvent)
    {
        _logger.LogInformation("Starting to process Order {OrderId}", orderCreatedEvent.OrderId);

        using (var scope = _serviceProvider.CreateScope())
        {
            var processOrderUseCase = scope.ServiceProvider.GetRequiredService<IProcessOrderUseCase>();

            await processOrderUseCase.Execute(orderCreatedEvent.OrderId.ToString());
        }

        _logger.LogInformation("Order {OrderId} processing completed!", orderCreatedEvent.OrderId);
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
