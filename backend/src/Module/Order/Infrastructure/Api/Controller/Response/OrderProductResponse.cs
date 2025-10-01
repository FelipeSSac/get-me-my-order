namespace Order.Infrastructure.Api.Controller.Response;

public record OrderProductResponse(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPriceAmount,
    string UnitPriceCurrency
);