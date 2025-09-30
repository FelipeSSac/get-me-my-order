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

    public async Task<Domain.Entity.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => EF.Property<Guid>(p, "Id") == id, cancellationToken);
    }

    public async Task<Domain.Entity.Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => EF.Property<string>(EF.Property<object>(p, "Name"), "Value") == name, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entity.Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entity.Product> AddAsync(Domain.Entity.Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task UpdateAsync(Domain.Entity.Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
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