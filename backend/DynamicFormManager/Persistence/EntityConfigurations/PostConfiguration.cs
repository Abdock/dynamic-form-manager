using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Conversions;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(e => e.Id);
        builder.HasOne(e => e.User)
            .WithMany(e => e.Posts)
            .HasForeignKey(e => e.UserId);
        builder.Property(e => e.Node)
            .HasConversion<JsonConverter>();
    }
}