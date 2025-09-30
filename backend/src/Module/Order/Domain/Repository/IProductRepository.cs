namespace Order.Domain.Repository;

public interface IProductRepository
{
    Task<Entity.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entity.Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entity.Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Entity.Product> AddAsync(Entity.Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entity.Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}