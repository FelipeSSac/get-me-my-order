namespace Order.Application.Service;

public interface IOrderNotificationService
{
    Task NotifyOrderStatusChanged(string orderId, string status, object? additionalData = null);
}