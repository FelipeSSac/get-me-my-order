using Order.Application.DTO;
using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;

namespace Order.Application.UseCase;

public class GetOrdersUseCase : IGetOrdersUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PaginatedResult<OrderEntity>> Execute(int page, int pageSize, Domain.Enum.OrderStatus? status = null)
    {
        if (page < 1)
            throw new ArgumentException("Page must be greater than 0", nameof(page));

        if (pageSize < 1 || pageSize > 100)
            throw new ArgumentException("PageSize must be between 1 and 100", nameof(pageSize));

        var (items, totalCount) = await _orderRepository.GetPaginatedAsync(page, pageSize, status);

        return new PaginatedResult<OrderEntity>(items, page, pageSize, totalCount);
    }
}