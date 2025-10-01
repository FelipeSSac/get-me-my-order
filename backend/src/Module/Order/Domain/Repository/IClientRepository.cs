using Order.Domain.Entity;

namespace Order.Domain.Repository;

public interface IClientRepository
{
    Task<bool> ClientExists(Guid id, CancellationToken cancellationToken = default);
    Task<ClientEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClientEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClientEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClientEntity> AddAsync(ClientEntity clientEntity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ClientEntity clientEntity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}