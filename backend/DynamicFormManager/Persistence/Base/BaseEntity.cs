using Persistence.Utilities;

namespace Persistence.Base;

public abstract class BaseEntity
{
    public Guid Id { get; init; } = Identifier.GenerateGuid();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}