namespace Presentation.DTO.Inputs.Auth;

public record LoginInput
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}