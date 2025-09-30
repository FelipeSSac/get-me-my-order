using Microsoft.EntityFrameworkCore;
using Order.Domain.Repository;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Persistence.EntityFramework.Repository;

public class OrderRepositoryEntityFramework : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepositoryEntityFramework(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entity.Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(o => EF.Property<Guid>(o, "Id") == id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entity.Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entity.Order>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => EF.Property<Guid>(o, "ClientId") == clientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entity.Order>> GetByStatusAsync(Domain.Enum.OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => EF.Property<Domain.Enum.OrderStatus>(o, "Status") == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entity.Order> AddAsync(Domain.Entity.Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task UpdateAsync(Domain.Entity.Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await GetByIdAsync(id, cancellationToken);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}