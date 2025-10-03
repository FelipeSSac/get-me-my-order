using Order.Domain.Entity;

namespace Order.Domain.Repository;

public interface IOrderRepository
{
    Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<OrderEntity> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, Enum.OrderStatus? status = null, CancellationToken cancellationToken = default);
    Task<OrderEntity> AddAsync(OrderEntity orderEntity, CancellationToken cancellationToken = default);
    Task UpdateAsync(OrderEntity orderEntity, CancellationToken cancellationToken = default);
}