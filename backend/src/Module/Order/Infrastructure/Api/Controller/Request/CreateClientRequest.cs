namespace Order.Infrastructure.Api.Controller.Request;

public record CreateClientRequest(
    string FirstName,
    string LastName,
    string Email 
);