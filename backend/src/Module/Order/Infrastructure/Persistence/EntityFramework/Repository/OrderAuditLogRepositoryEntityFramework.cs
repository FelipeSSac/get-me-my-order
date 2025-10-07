using Microsoft.EntityFrameworkCore;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Domain.Repository;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Persistence.EntityFramework.Repository;

public class OrderAuditLogRepositoryEntityFramework : IOrderAuditLogRepository
{
    private readonly OrderDbContext _context;

    public OrderAuditLogRepositoryEntityFramework(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<OrderAuditLogEntity> Items, int TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        OrderAuditAction? action = null,
        Guid? orderId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.OrderAuditLogs
            .AsNoTracking()
            .AsQueryable();

        if (action.HasValue)
        {
            query = query.Where(a => EF.Property<OrderAuditAction>(a, "Action") == action.Value);
        }

        if (orderId.HasValue)
        {
            query = query.Where(a => EF.Property<Guid>(a, "OrderId") == orderId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(a => EF.Property<DateTime>(a, "CreatedAt"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IEnumerable<OrderAuditLogEntity>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.OrderAuditLogs
            .AsNoTracking()
            .Where(a => EF.Property<Guid>(a, "OrderId") == orderId)
            .OrderByDescending(a => EF.Property<DateTime>(a, "CreatedAt"))
            .ToListAsync(cancellationToken);
    }

    public async Task<OrderAuditLogEntity> AddAsync(
        OrderAuditLogEntity orderAuditLog,
        CancellationToken cancellationToken = default)
    {
        await _context.OrderAuditLogs.AddAsync(orderAuditLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return orderAuditLog;
    }
}