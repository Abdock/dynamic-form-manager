using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Context;

public class FormContext : DbContext
{
    public DbSet<Post> Posts { get; init; }

    protected FormContext()
    {
    }

    public FormContext(DbContextOptions options) : base(options)
    {
    }
}