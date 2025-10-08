namespace Order.Infrastructure.Api.Controller.Response;

public record ClientResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email, 
    DateTime CreatedAt,
    DateTime UpdatedAt
);