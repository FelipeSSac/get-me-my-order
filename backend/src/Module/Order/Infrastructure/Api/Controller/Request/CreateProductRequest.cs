namespace Order.Infrastructure.Api.Controller.Request;

public record CreateProductRequest(
    string Name,
    decimal Value,
    string Currency 
);