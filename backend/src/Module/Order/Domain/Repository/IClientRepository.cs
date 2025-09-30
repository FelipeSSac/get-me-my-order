namespace Order.Domain.Repository;

public interface IClientRepository
{
    Task<Entity.Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Entity.Client?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entity.Client>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Entity.Client> AddAsync(Entity.Client client, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entity.Client client, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}