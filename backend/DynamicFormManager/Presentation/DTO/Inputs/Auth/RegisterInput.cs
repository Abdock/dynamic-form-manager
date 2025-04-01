namespace Presentation.DTO.Inputs.Auth;

public record RegisterInput
{
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}