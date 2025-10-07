using Order.Domain.Entity;
using Order.Domain.Enum;

namespace Order.Domain.Repository;

public interface IOrderAuditLogRepository
{
    Task<(IEnumerable<OrderAuditLogEntity> Items, int TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        OrderAuditAction? action = null,
        Guid? orderId = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<OrderAuditLogEntity>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<OrderAuditLogEntity> AddAsync(
        OrderAuditLogEntity orderAuditLog,
        CancellationToken cancellationToken = default);
}