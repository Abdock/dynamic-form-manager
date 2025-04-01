namespace Application.DTO.Requests.Auth;

public record AuthorizedUserRequest
{
    public required Guid UserId { get; init; }
}