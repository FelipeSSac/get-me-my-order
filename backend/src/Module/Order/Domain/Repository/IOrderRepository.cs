namespace Order.Domain.Repository;

public interface IOrderRepository
{
    Task<Entity.Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entity.Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Entity.Order>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entity.Order>> GetByStatusAsync(Enum.OrderStatus status, CancellationToken cancellationToken = default);
    Task<Entity.Order> AddAsync(Entity.Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entity.Order order, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}