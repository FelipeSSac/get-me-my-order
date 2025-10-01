using Microsoft.EntityFrameworkCore;
using Order.Domain.Repository;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Persistence.EntityFramework.Repository;

public class ClientRepositoryEntityFramework : IClientRepository
{
    private readonly OrderDbContext _context;

    public ClientRepositoryEntityFramework(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ClientExists(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .AnyAsync(c => EF.Property<Guid>(c, "Id") == id, cancellationToken);
    }
    
    public async Task<Domain.Entity.ClientEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => EF.Property<Guid>(c, "Id") == id, cancellationToken);
    }

    public async Task<Domain.Entity.ClientEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => EF.Property<string>(EF.Property<object>(c, "Email"), "Value") == email, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entity.ClientEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clients.ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entity.ClientEntity> AddAsync(Domain.Entity.ClientEntity clientEntity, CancellationToken cancellationToken = default)
    {
        await _context.Clients.AddAsync(clientEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return clientEntity;
    }

    public async Task UpdateAsync(Domain.Entity.ClientEntity clientEntity, CancellationToken cancellationToken = default)
    {
        _context.Clients.Update(clientEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await GetByIdAsync(id, cancellationToken);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}