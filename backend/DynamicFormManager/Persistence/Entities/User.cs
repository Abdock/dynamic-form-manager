using Persistence.Base;

namespace Persistence.Entities;

public class User : BaseEntity
{
    public required string Email { get; init; }
    public required string PasswordHash { get; init; }
    public required string Username { get; init; }
    public IReadOnlyCollection<Post> Posts { get; init; } = new List<Post>();
}