namespace Application.Services.Jwt.Models;

public record UserClaims
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
}