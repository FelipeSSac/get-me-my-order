using Order.Domain.Enum;

namespace Order.Domain.Entity;

public class OrderAuditLogEntity
{
    private Guid? Id { get; }
    private Guid OrderId { get; }
    private OrderAuditAction Action { get; }
    private OrderStatus? OldStatus { get; }
    private OrderStatus? NewStatus { get; }
    private decimal? OldTotalValue { get; }
    private decimal? NewTotalValue { get; }
    private string? Currency { get; }
    private string? ChangedBy { get; }
    private string? Reason { get; }
    private string? Metadata { get; }
    private DateTime CreatedAt { get; } = DateTime.UtcNow;

    private OrderAuditLogEntity() {}

    private OrderAuditLogEntity(
        Guid? id,
        Guid orderId,
        OrderAuditAction action,
        OrderStatus? oldStatus,
        OrderStatus? newStatus,
        decimal? oldTotalValue,
        decimal? newTotalValue,
        string? currency,
        string? changedBy,
        string? reason,
        string? metadata)
    {
        Id = id;
        OrderId = orderId;
        Action = action;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        OldTotalValue = oldTotalValue;
        NewTotalValue = newTotalValue;
        Currency = currency;
        ChangedBy = changedBy;
        Reason = reason;
        Metadata = metadata;
    }

    public static OrderAuditLogEntity CreateStatusChange(
        Guid orderId,
        OrderStatus oldStatus,
        OrderStatus newStatus,
        string? changedBy = null,
        string? reason = null)
    {
        return new OrderAuditLogEntity(
            id: null,
            orderId: orderId,
            action: OrderAuditAction.StatusChanged,
            oldStatus: oldStatus,
            newStatus: newStatus,
            oldTotalValue: null,
            newTotalValue: null,
            currency: null,
            changedBy: changedBy,
            reason: reason,
            metadata: null
        );
    }

    public static OrderAuditLogEntity CreateOrderCreated(
        Guid orderId,
        decimal totalValue,
        string currency,
        string? changedBy = null)
    {
        return new OrderAuditLogEntity(
            id: null,
            orderId: orderId,
            action: OrderAuditAction.OrderCreated,
            oldStatus: null,
            newStatus: OrderStatus.Pending,
            oldTotalValue: null,
            newTotalValue: totalValue,
            currency: currency,
            changedBy: changedBy,
            reason: null,
            metadata: null
        );
    }

    public static OrderAuditLogEntity CreateValueChange(
        Guid orderId,
        decimal oldValue,
        decimal newValue,
        string currency,
        string? changedBy = null,
        string? reason = null)
    {
        return new OrderAuditLogEntity(
            id: null,
            orderId: orderId,
            action: OrderAuditAction.ValueChanged,
            oldStatus: null,
            newStatus: null,
            oldTotalValue: oldValue,
            newTotalValue: newValue,
            currency: currency,
            changedBy: changedBy,
            reason: reason,
            metadata: null
        );
    }

    public static OrderAuditLogEntity CreateCustomAction(
        Guid orderId,
        OrderAuditAction action,
        string? metadata = null,
        string? changedBy = null,
        string? reason = null)
    {
        return new OrderAuditLogEntity(
            id: null,
            orderId: orderId,
            action: action,
            oldStatus: null,
            newStatus: null,
            oldTotalValue: null,
            newTotalValue: null,
            currency: null,
            changedBy: changedBy,
            reason: reason,
            metadata: metadata
        );
    }

    public Guid? GetId() => Id;
    public Guid GetOrderId() => OrderId;
    public OrderAuditAction GetAction() => Action;
    public OrderStatus? GetOldStatus() => OldStatus;
    public OrderStatus? GetNewStatus() => NewStatus;
    public decimal? GetOldTotalValue() => OldTotalValue;
    public decimal? GetNewTotalValue() => NewTotalValue;
    public string? GetCurrency() => Currency;
    public string? GetChangedBy() => ChangedBy;
    public string? GetReason() => Reason;
    public string? GetMetadata() => Metadata;
    public DateTime GetCreatedAt() => CreatedAt;
}