using Order.Application.UseCase.Interface;
using Order.Domain.Repository;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Request;
using Order.Infrastructure.Data;

namespace Order.Application.UseCase;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IClientRepository _clientRepository;
    private readonly OrderDbContext _context;

    public CreateOrderUseCase(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IClientRepository clientRepository,
        OrderDbContext context)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _clientRepository = clientRepository;
        _context = context;
    }

    public async Task<OrderEntity> Execute(CreateOrderRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            ClientEntity? client = await _clientRepository.GetByIdAsync(request.ClientId);

            if (client == null)
                throw new ArgumentException("Client does not exists!");

            Guid orderId = new Guid();

            List<OrderProductEntity> orderProducts = await _createProducts(orderId, request.ProductList);

            OrderEntity order = OrderEntity.Create(
                null,
                client,
                orderProducts,
                OrderStatus.Pending
            );

            var result = await _orderRepository.AddAsync(order);

            await transaction.CommitAsync();

            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<List<OrderProductEntity>> _createProducts(Guid orderId, List<CreateOrderRequestProduct> productList)
    {
        List<OrderProductEntity> orderProducts = new List<OrderProductEntity>();
        
        foreach (var orderProduct in productList)
        {
            ProductEntity? product = await _productRepository.GetByIdAsync(orderProduct.ProductId);

            if (product == null)
                throw new ArgumentException("Product does not exists!");
            
            orderProducts.Add(OrderProductEntity.Create(null, orderId, product, orderProduct.Quantity, product.GetValue()));
        }

        return orderProducts;
    }
}