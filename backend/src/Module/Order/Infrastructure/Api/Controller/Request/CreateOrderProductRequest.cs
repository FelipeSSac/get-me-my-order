namespace Order.Infrastructure.Api.Controller.Request;

public record CreateOrderProductRequest(
    Guid ProductId,
    int Quantity
);