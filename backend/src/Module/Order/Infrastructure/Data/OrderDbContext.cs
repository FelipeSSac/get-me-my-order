using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Persistence.EntityFramework.Configurations;

namespace Order.Infrastructure.Data;

public class OrderDbContext : DbContext
{
    public DbSet<Domain.Entity.Order> Orders => Set<Domain.Entity.Order>();
    public DbSet<Domain.Entity.Client> Clients => Set<Domain.Entity.Client>();
    public DbSet<Domain.Entity.Product> Products => Set<Domain.Entity.Product>();

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }
}