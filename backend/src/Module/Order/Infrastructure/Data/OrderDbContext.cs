using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Persistence.EntityFramework.Configurations;

namespace Order.Infrastructure.Data;

public class OrderDbContext : DbContext
{
    public DbSet<Domain.Entity.OrderEntity> Orders => Set<Domain.Entity.OrderEntity>();
    public DbSet<Domain.Entity.ClientEntity> Clients => Set<Domain.Entity.ClientEntity>();
    public DbSet<Domain.Entity.ProductEntity> Products => Set<Domain.Entity.ProductEntity>();
    public DbSet<Domain.Entity.OrderProductEntity> OrderProducts => Set<Domain.Entity.OrderProductEntity>();

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
    }
}