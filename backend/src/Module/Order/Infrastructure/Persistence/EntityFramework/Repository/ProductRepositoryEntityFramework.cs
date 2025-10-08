using Microsoft.EntityFrameworkCore;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Persistence.EntityFramework.Repository;

public class ProductRepositoryEntityFramework : IProductRepository
{
    private readonly OrderDbContext _context;

    public ProductRepositoryEntityFramework(OrderDbContext context)
    {
        _context = context;
    }
    
    public async Task<(IEnumerable<ProductEntity> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .AsQueryable();
        

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(o => EF.Property<DateTime>(o, "CreatedAt"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => EF.Property<Guid>(p, "Id") == id, cancellationToken);
    }

    public async Task<ProductEntity> AddAsync(ProductEntity productEntity, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(productEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return productEntity;
    }
}