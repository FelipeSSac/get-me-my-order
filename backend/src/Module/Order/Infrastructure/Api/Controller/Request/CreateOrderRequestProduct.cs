namespace Order.Infrastructure.Api.Controller.Request;

public sealed class CreateOrderRequestProduct
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}