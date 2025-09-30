using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infrastructure.Persistence.EntityFramework.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Domain.Entity.Client>
{
    public void Configure(EntityTypeBuilder<Domain.Entity.Client> builder)
    {
        builder.ToTable("clients");

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

        // Value object - PersonName
        builder.OwnsOne<Domain.ValueObject.PersonName>("Name", name =>
        {
            name.Property("FirstName")
                .HasColumnName("first_name")
                .HasMaxLength(50)
                .IsRequired();

            name.Property("LastName")
                .HasColumnName("last_name")
                .HasMaxLength(50)
                .IsRequired();
        });

        // Value object - Email
        builder.OwnsOne<Domain.ValueObject.Email>("Email", email =>
        {
            email.Property("Value")
                .HasColumnName("email")
                .HasMaxLength(254)
                .IsRequired();

            email.HasIndex("Value")
                .IsUnique();
        });
    }
}