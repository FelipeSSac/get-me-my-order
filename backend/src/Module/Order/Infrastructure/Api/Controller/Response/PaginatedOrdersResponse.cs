namespace Order.Infrastructure.Api.Controller.Response;

public record PaginatedOrdersResponse(
    IEnumerable<OrderResponse> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);