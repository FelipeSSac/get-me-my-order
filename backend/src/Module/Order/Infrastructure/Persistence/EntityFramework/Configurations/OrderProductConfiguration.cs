using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infrastructure.Persistence.EntityFramework.Configurations;

public class OrderProductConfiguration : IEntityTypeConfiguration<Domain.Entity.OrderProductEntity>
{
    public void Configure(EntityTypeBuilder<Domain.Entity.OrderProductEntity> builder)
    {
        builder.ToTable("order_products");

        builder.HasKey("Id");
        builder.Property("Id")
            .HasColumnName("id")
            .IsRequired();

        builder.Property("OrderId")
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property("ProductId")
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property("Quantity")
            .HasColumnName("quantity")
            .IsRequired();

        // Value object - Money (UnitPrice)
        builder.OwnsOne<Domain.ValueObject.Money>("UnitPrice", money =>
        {
            money.Property("Amount")
                .HasColumnName("unit_price_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property("Currency")
                .HasColumnName("unit_price_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property("CreatedAt")
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property("UpdatedAt")
            .HasColumnName("updated_at")
            .IsRequired();

        // Relationships
        builder.HasOne<Domain.Entity.OrderEntity>("Order")
            .WithMany("OrderProducts")
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Domain.Entity.ProductEntity>("Product")
            .WithMany()
            .HasForeignKey("ProductId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("OrderId", "ProductId")
            .IsUnique();
    }
}