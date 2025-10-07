using Order.Application.UseCase.Interface;
using Order.Application.Service;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Order.Domain.Event;
using Microsoft.Extensions.Logging;

namespace Order.Application.UseCase;

public class ProcessOrderUseCase : IProcessOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderNotificationService _notificationService;
    private readonly IDomainEventPublisherService _eventPublisherService;
    private readonly ILogger<ProcessOrderUseCase> _logger;
    private readonly int _processingTimeInMs = 5000;

    public ProcessOrderUseCase(
        IOrderRepository orderRepository,
        IOrderNotificationService notificationService,
        IDomainEventPublisherService eventPublisherService,
        ILogger<ProcessOrderUseCase> logger)
    {
        _orderRepository = orderRepository;
        _notificationService = notificationService;
        _eventPublisherService = eventPublisherService;
        _logger = logger;
    }
    
    public async Task Execute(string id)
    {
        _logger.LogInformation("[ProcessOrderUseCase::Execute] Starting to process order {OrderId}", id);

        OrderEntity? order = await _orderRepository.GetByIdAsync(new Guid(id));

        if (order == null)
        {
            _logger.LogWarning("[ProcessOrderUseCase::Execute] Order {OrderId} not found for processing", id);
            throw new Exception("Order not found!");
        }

        order = await SetOrderAsProcessing(order);

        _logger.LogInformation("[ProcessOrderUseCase::Execute] Order {OrderId} is now processing. Waiting {ProcessingTime}ms",
            id, _processingTimeInMs);

        Thread.Sleep(_processingTimeInMs);

        await SetOrderAsDone(order);

        _logger.LogInformation("[ProcessOrderUseCase::Execute] Order {OrderId} processing completed successfully", id);
    }

    private async Task<OrderEntity> SetOrderAsProcessing(OrderEntity order)
    {
        _logger.LogInformation("[ProcessOrderUseCase::SetOrderAsProcessing] Setting order {OrderId} status to Processing", order.GetId());

        var oldStatus = order.GetStatus();
        order = order.SetAsProcessing();
        await _orderRepository.UpdateAsync(order);

        await _notificationService.NotifyOrderStatusChanged(
            order.GetId().ToString()!,
            order.GetStatus().ToString(),
            new
            {
                totalValue = order.GetTotalValue().GetAmount(),
                currency = order.GetTotalValue().GetCurrency(),
                updatedAt = order.GetUpdatedAt()
            }
        );

        var orderStatusChangedEvent = new OrderStatusChangedEvent(
            order.GetId() ?? Guid.Empty,
            order.GetClientId(),
            order.GetTotalValue().GetAmount(),
            order.GetTotalValue().GetCurrency(),
            oldStatus,
            order.GetStatus(),
            DateTime.UtcNow
        );

        await _eventPublisherService.PublishAsync(orderStatusChangedEvent);

        _logger.LogInformation("[ProcessOrderUseCase::SetOrderAsProcessing] Order {OrderId} status updated to Processing, notification and event dispatched", order.GetId());

        return order;
    }

    private async Task<OrderEntity> SetOrderAsDone(OrderEntity order)
    {
        _logger.LogInformation("[ProcessOrderUseCase::SetOrderAsDone] Setting order {OrderId} status to Done", order.GetId());

        var oldStatus = order.GetStatus();
        order = order.SetAsDone();
        await _orderRepository.UpdateAsync(order);

        await _notificationService.NotifyOrderStatusChanged(
            order.GetId().ToString()!,
            order.GetStatus().ToString(),
            new
            {
                totalValue = order.GetTotalValue().GetAmount(),
                currency = order.GetTotalValue().GetCurrency(),
                updatedAt = order.GetUpdatedAt()
            }
        );

        var orderStatusChangedEvent = new OrderStatusChangedEvent(
            order.GetId() ?? Guid.Empty,
            order.GetClientId(),
            order.GetTotalValue().GetAmount(),
            order.GetTotalValue().GetCurrency(),
            oldStatus,
            order.GetStatus(),
            DateTime.UtcNow
        );

        await _eventPublisherService.PublishAsync(orderStatusChangedEvent);

        _logger.LogInformation("[ProcessOrderUseCase::SetOrderAsDone] Order {OrderId} status updated to Done, notification and event dispatched", order.GetId());

        return order;
    }
}