namespace Order.Infrastructure.Api.Controller.Response;

public record OrderResponse(
    Guid Id,
    Guid ClientId,
    List<OrderProductResponse> Products,
    decimal TotalAmount,
    string TotalCurrency,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);