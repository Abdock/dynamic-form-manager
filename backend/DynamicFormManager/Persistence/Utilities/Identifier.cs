namespace Persistence.Utilities;

public static class Identifier
{
    public static Guid GenerateGuid() => Ulid.NewUlid().ToGuid();
}