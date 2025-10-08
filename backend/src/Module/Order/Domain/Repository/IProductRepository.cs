using Order.Domain.Entity;

namespace Order.Domain.Repository;

public interface IProductRepository
{
    Task<(IEnumerable<ProductEntity> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductEntity> AddAsync(ProductEntity productEntity, CancellationToken cancellationToken = default);
}