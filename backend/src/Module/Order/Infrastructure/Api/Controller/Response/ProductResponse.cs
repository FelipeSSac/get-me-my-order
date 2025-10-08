namespace Order.Infrastructure.Api.Controller.Response;

public record ProductResponse(
    Guid Id, 
    string Name, 
    decimal Price, 
    string Currency,
    DateTime CreatedAt,
    DateTime UpdatedAt
);