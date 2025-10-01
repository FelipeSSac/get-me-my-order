namespace Order.Infrastructure.Api.Controller.Request;

public sealed class CreateOrderRequest
{
    public List<CreateOrderRequestProduct> ProductList { get; set; }
    public Guid ClientId { get; set; }
}