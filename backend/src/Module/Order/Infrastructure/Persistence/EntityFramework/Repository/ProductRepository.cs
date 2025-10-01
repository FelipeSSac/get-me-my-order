using Microsoft.EntityFrameworkCore;
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
    
    public async Task<bool> ProductExists(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AnyAsync(c => EF.Property<Guid>(c, "Id") == id, cancellationToken);
    }

    public async Task<Domain.Entity.ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => EF.Property<Guid>(p, "Id") == id, cancellationToken);
    }

    public async Task<Domain.Entity.ProductEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => EF.Property<string>(EF.Property<object>(p, "Name"), "Value") == name, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entity.ProductEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entity.ProductEntity> AddAsync(Domain.Entity.ProductEntity productEntity, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(productEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return productEntity;
    }

    public async Task UpdateAsync(Domain.Entity.ProductEntity productEntity, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(productEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}