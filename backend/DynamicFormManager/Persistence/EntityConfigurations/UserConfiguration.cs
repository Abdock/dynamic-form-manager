using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Email)
            .HasMaxLength(255);
        builder.Property(e => e.Username)
            .HasMaxLength(128);
        builder.Property(e => e.PasswordHash)
            .HasMaxLength(100);
    }
}