using Microsoft.AspNetCore.SignalR;
using Order.Application.Service;
using Worker.Hubs;

namespace Worker.Services;

public class OrderNotificationService : IOrderNotificationService
{
    private readonly IHubContext<OrderHub> _hubContext;
    private readonly ILogger<OrderNotificationService> _logger;

    public OrderNotificationService(
        IHubContext<OrderHub> hubContext,
        ILogger<OrderNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifyOrderStatusChanged(string orderId, string status, object? additionalData = null)
    {
        try
        {
            var notification = new
            {
                orderId,
                status,
                timestamp = DateTime.UtcNow,
                data = additionalData
            };

            await _hubContext.Clients
                .Group($"order-{orderId}")
                .SendAsync("OrderStatusChanged", notification);

            _logger.LogInformation(
                "[OrderNotificationService::NotifyOrderStatusChanged] Notification sent for order {OrderId} with status {Status}",
                orderId,
                status
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "[OrderNotificationService::NotifyOrderStatusChanged] Error sending notification for order {OrderId}",
                orderId
            );
        }
    }
}