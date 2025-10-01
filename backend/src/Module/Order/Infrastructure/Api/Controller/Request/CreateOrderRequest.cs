namespace Order.Infrastructure.Api.Controller.Request;

public record CreateOrderRequest(
    List<CreateOrderProductRequest> ProductList,
    Guid ClientId
);