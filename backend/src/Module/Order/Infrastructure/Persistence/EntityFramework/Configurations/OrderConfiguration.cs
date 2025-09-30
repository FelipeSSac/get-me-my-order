using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Enum;

namespace Order.Infrastructure.Persistence.EntityFramework.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entity.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entity.Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey("Id");
        builder.Property("Id")
            .HasColumnName("id")
            .IsRequired();

        builder.Property("Status")
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property("CreatedAt")
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property("UpdatedAt")
            .HasColumnName("updated_at")
            .IsRequired();

        // Client relationship
        builder.HasOne(typeof(Domain.Entity.Client), "Client")
            .WithMany()
            .HasForeignKey("ClientId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<Guid>("ClientId")
            .HasColumnName("client_id")
            .IsRequired();

        // Product relationship
        builder.HasOne(typeof(Domain.Entity.Product), "Product")
            .WithMany()
            .HasForeignKey("ProductId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<Guid>("ProductId")
            .HasColumnName("product_id")
            .IsRequired();

        // Value object - Money
        builder.OwnsOne<Domain.ValueObject.Money>("Value", money =>
        {
            money.Property("Amount")
                .HasColumnName("value_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property("Currency")
                .HasColumnName("value_currency")
                .HasMaxLength(3)
                .IsRequired();
        });
    }
}