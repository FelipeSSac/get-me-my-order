using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Enum;

namespace Order.Infrastructure.Persistence.EntityFramework.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entity.OrderEntity>
{
    public void Configure(EntityTypeBuilder<Domain.Entity.OrderEntity> builder)
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
        builder.HasOne(typeof(Domain.Entity.ClientEntity), "ClientEntity")
            .WithMany()
            .HasForeignKey("ClientId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<Guid>("ClientId")
            .HasColumnName("client_id")
            .IsRequired();

        // Value object - Money
        builder.OwnsOne<Domain.ValueObject.Money>("TotalValue", money =>
        {
            money.Property("Amount")
                .HasColumnName("total_value_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property("Currency")
                .HasColumnName("total_value_currency")
                .HasMaxLength(3)
                .IsRequired();
        });
    }
}