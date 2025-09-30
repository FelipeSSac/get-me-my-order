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

    public async Task<Domain.Entity.Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => EF.Property<Guid>(c, "Id") == id, cancellationToken);
    }

    public async Task<Domain.Entity.Client?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => EF.Property<string>(EF.Property<object>(c, "Email"), "Value") == email, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entity.Client>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clients.ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entity.Client> AddAsync(Domain.Entity.Client client, CancellationToken cancellationToken = default)
    {
        await _context.Clients.AddAsync(client, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return client;
    }

    public async Task UpdateAsync(Domain.Entity.Client client, CancellationToken cancellationToken = default)
    {
        _context.Clients.Update(client);
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