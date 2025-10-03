using Order.Domain.Entity;

namespace Order.Infrastructure.Api.Controller.Response;

public record OrderResponse(
    Guid Id,
    ClientResponse Client,
    List<OrderProductResponse> Products,
    decimal TotalAmount,
    string TotalCurrency,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);