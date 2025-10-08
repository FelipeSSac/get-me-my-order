using Microsoft.EntityFrameworkCore;
using Order.Domain.Entity;
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
    
    public async Task<(IEnumerable<ClientEntity> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Clients
            .AsQueryable();
        

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(o => EF.Property<DateTime>(o, "CreatedAt"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
    
    public async Task<ClientEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => EF.Property<Guid>(c, "Id") == id, cancellationToken);
    }

    public async Task<ClientEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => EF.Property<string>(EF.Property<object>(c, "Email"), "Value") == email, cancellationToken);
    }

    public async Task<ClientEntity> AddAsync(ClientEntity clientEntity, CancellationToken cancellationToken = default)
    {
        await _context.Clients.AddAsync(clientEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return clientEntity;
    }
}