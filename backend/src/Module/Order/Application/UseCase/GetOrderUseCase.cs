using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace Order.Application.UseCase;

public class GetOrderUseCase : IGetOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<GetOrderUseCase> _logger;

    public GetOrderUseCase(
        IOrderRepository orderRepository,
        ILogger<GetOrderUseCase> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<OrderEntity?> Execute(string id)
    {
        _logger.LogInformation("[GetOrderUseCase::Execute] Fetching order with id {OrderId}", id);

        bool isIdGuid = Guid.TryParse(id.ToString(), out Guid idGuid);

        if (!isIdGuid)
        {
            _logger.LogWarning("[GetOrderUseCase::Execute] Invalid order id format: {OrderId}", id);
            throw new ArgumentException("Invalid order id");
        }

        var order = await _orderRepository.GetByIdAsync(idGuid);

        if (order == null)
        {
            _logger.LogWarning("[GetOrderUseCase::Execute] Order {OrderId} not found", id);
        }
        else
        {
            _logger.LogInformation("[GetOrderUseCase::Execute] Order {OrderId} found with status {Status}",
                id, order.GetStatus());
        }

        return order;
    }
}