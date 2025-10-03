using Microsoft.EntityFrameworkCore;
using Order.Domain.Entity;
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

    public async Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include("ClientEntity")
            .Include("OrderProducts.Product")
            .FirstOrDefaultAsync(o => EF.Property<Guid>(o, "Id") == id, cancellationToken);
    }

    public async Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include("ClientEntity")
            .Include("OrderProducts.Product")
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderEntity>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include("ClientEntity")
            .Include("OrderProducts.Product")
            .Where(o => EF.Property<Guid>(o, "ClientId") == clientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderEntity>> GetByStatusAsync(Domain.Enum.OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include("ClientEntity")
            .Include("OrderProducts.Product")
            .Where(o => EF.Property<Domain.Enum.OrderStatus>(o, "Status") == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<OrderEntity> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, Domain.Enum.OrderStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Orders
            .Include("ClientEntity")
            .Include("OrderProducts.Product")
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(o => EF.Property<Domain.Enum.OrderStatus>(o, "Status") == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(o => EF.Property<DateTime>(o, "CreatedAt"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<OrderEntity> AddAsync(OrderEntity orderEntity, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(orderEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return orderEntity;
    }

    public async Task UpdateAsync(OrderEntity orderEntity, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(orderEntity);
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