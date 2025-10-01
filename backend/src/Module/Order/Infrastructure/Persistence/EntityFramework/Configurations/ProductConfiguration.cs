using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infrastructure.Persistence.EntityFramework.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Domain.Entity.ProductEntity>
{
    public void Configure(EntityTypeBuilder<Domain.Entity.ProductEntity> builder)
    {
        builder.ToTable("products");

        builder.HasKey("Id");
        builder.Property("Id")
            .HasColumnName("id")
            .IsRequired();

        builder.Property("CreatedAt")
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property("UpdatedAt")
            .HasColumnName("updated_at")
            .IsRequired();

        // Value object - ProductName
        builder.OwnsOne<Domain.ValueObject.ProductName>("Name", name =>
        {
            name.Property("Value")
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();
        });

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