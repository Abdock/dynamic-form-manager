using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Context;

public class FormContext : DbContext
{
    public DbSet<Post> Posts { get; init; }
    public DbSet<User> Users { get; init; }

    protected FormContext()
    {
    }

    public FormContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FormContext).Assembly);
    }
}