using Order.Application.UseCase.Interface;
using Order.Domain.Repository;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Domain.Event;
using Order.Infrastructure.Api.Controller.Request;
using Order.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Order.Application.Service;

namespace Order.Application.UseCase;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IClientRepository _clientRepository;
    private readonly OrderDbContext _context;
    private readonly IDomainEventPublisherService _eventPublisherService;
    private readonly ILogger<CreateOrderUseCase> _logger;

    public CreateOrderUseCase(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IClientRepository clientRepository,
        OrderDbContext context,
        IDomainEventPublisherService eventPublisherService,
        ILogger<CreateOrderUseCase> logger
    ) {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _clientRepository = clientRepository;
        _context = context;
        _eventPublisherService = eventPublisherService;
        _logger = logger;
    }

    public async Task<OrderEntity> Execute(CreateOrderRequest request)
    {
        _logger.LogInformation("[CreateOrderUseCase::Execute] Creating order for client {ClientId} with {ProductCount} products",
            request.ClientId, request.ProductList.Count);

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            ClientEntity? client = await _clientRepository.GetByIdAsync(request.ClientId);

            if (client == null)
            {
                _logger.LogWarning("[CreateOrderUseCase::Execute] Client {ClientId} not found", request.ClientId);
                throw new ArgumentException("Client does not exists!");
            }

            Guid orderId = new Guid();

            List<OrderProductEntity> orderProducts = await _createProducts(orderId, request.ProductList);

            OrderEntity order = OrderEntity.Create(
                null,
                client,
                orderProducts,
                OrderStatus.Pending
            );

            await _orderRepository.AddAsync(order);

            _logger.LogInformation("[CreateOrderUseCase::Execute] Order {OrderId} created with total value {TotalValue} {Currency}",
                order.GetId(), order.GetTotalValue().GetAmount(), order.GetTotalValue().GetCurrency());

            var orderCreatedEvent = new OrderCreatedEvent(
                order.GetId() ?? Guid.Empty,
                order.GetClientId(),
                order.GetTotalValue().GetAmount(),
                order.GetTotalValue().GetCurrency(),
                order.GetCreatedAt()
            );

            await _eventPublisherService.PublishAsync(orderCreatedEvent);

            _logger.LogInformation("[CreateOrderUseCase::Execute] OrderCreatedEvent published for order {OrderId}", order.GetId());

            await transaction.CommitAsync();

            _logger.LogInformation("[CreateOrderUseCase::Execute] Order {OrderId} transaction committed successfully", order.GetId());

            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CreateOrderUseCase::Execute] Error creating order for client {ClientId}", request.ClientId);
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<List<OrderProductEntity>> _createProducts(Guid orderId, List<CreateOrderProductRequest> productList)
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