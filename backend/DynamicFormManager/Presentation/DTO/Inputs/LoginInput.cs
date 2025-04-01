namespace Presentation.DTO.Inputs;

public record LoginInput
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}