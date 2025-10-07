namespace Order.Domain.Enum;

public enum OrderAuditAction
{
    OrderCreated = 0,
    StatusChanged = 1,
    ValueChanged = 2,
    ProductsChanged = 3,
    CustomAction = 99
}