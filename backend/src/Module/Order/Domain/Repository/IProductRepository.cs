using Order.Domain.Entity;

namespace Order.Domain.Repository;

public interface IProductRepository
{
    Task<bool> ProductExists(Guid id, CancellationToken cancellationToken = default);
    Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductEntity> AddAsync(ProductEntity productEntity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductEntity productEntity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}