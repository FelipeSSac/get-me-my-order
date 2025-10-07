using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infrastructure.Persistence.EntityFramework.Configurations;

public class OrderAuditLogConfiguration : IEntityTypeConfiguration<Domain.Entity.OrderAuditLogEntity>
{
    public void Configure(EntityTypeBuilder<Domain.Entity.OrderAuditLogEntity> builder)
    {
        builder.ToTable("order_audit_logs");

        builder.HasKey("Id");
        builder.Property("Id")
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property("OrderId")
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property("Action")
            .HasColumnName("action")
            .HasConversion<int>()
            .IsRequired();

        builder.Property("OldStatus")
            .HasColumnName("old_status")
            .HasConversion<int?>();

        builder.Property("NewStatus")
            .HasColumnName("new_status")
            .HasConversion<int?>();

        builder.Property("OldTotalValue")
            .HasColumnName("old_total_value")
            .HasColumnType("decimal(18,2)");

        builder.Property("NewTotalValue")
            .HasColumnName("new_total_value")
            .HasColumnType("decimal(18,2)");

        builder.Property("Currency")
            .HasColumnName("currency")
            .HasMaxLength(3);

        builder.Property("ChangedBy")
            .HasColumnName("changed_by")
            .HasMaxLength(255);

        builder.Property("Reason")
            .HasColumnName("reason")
            .HasMaxLength(500);

        builder.Property("Metadata")
            .HasColumnName("metadata")
            .HasColumnType("text");

        builder.Property("CreatedAt")
            .HasColumnName("created_at")
            .IsRequired();

        // Indexes for better query performance
        builder.HasIndex("OrderId")
            .HasDatabaseName("idx_order_audit_logs_order_id");

        builder.HasIndex("Action")
            .HasDatabaseName("idx_order_audit_logs_action");

        builder.HasIndex("CreatedAt")
            .HasDatabaseName("idx_order_audit_logs_created_at");

        // Composite index for common query pattern
        builder.HasIndex("OrderId", "Action")
            .HasDatabaseName("idx_order_audit_logs_order_id_action");
    }
}