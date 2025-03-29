using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Context;

public class FromContext : DbContext
{
    public DbSet<Post> Posts { get; init; }

    protected FromContext()
    {
    }

    public FromContext(DbContextOptions options) : base(options)
    {
    }
}