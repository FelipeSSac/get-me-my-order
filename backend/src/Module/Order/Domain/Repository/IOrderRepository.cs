using Order.Domain.Entity;

namespace Order.Domain.Repository;

public interface IOrderRepository
{
    Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderEntity>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderEntity>> GetByStatusAsync(Enum.OrderStatus status, CancellationToken cancellationToken = default);
    Task<OrderEntity> AddAsync(OrderEntity orderEntity, CancellationToken cancellationToken = default);
    Task UpdateAsync(OrderEntity orderEntity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}