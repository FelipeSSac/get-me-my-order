using Order.Domain.Entity;
using Order.Infrastructure.Api.Controller.Response;

namespace Order.Infrastructure.Api.Controller.Mapper;

public static class OrderMapper
{
    public static OrderResponse ToResponse(this OrderEntity order)
    {
        return new OrderResponse(
            order.GetId() ?? Guid.Empty,
            order.GetClient().ToResponse(),
            order.GetOrderProducts().Select(p => new OrderProductResponse(
                p.GetProductId(),
                p.GetProductName(),
                p.GetQuantity(),
                p.GetUnitPrice().GetAmount(),
                p.GetUnitPrice().GetCurrency()
            )).ToList(),
            order.GetTotalValue().GetAmount(),
            order.GetTotalValue().GetCurrency(),
            order.GetStatus().ToString(),
            order.GetCreatedAt(),
            order.GetUpdatedAt()
        );
    }
}