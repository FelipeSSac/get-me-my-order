using Order.Domain.Entity;

namespace Order.Domain.Repository;

public interface IClientRepository
{
    Task<(IEnumerable<ClientEntity> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ClientEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClientEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<ClientEntity> AddAsync(ClientEntity clientEntity, CancellationToken cancellationToken = default);
}