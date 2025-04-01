namespace Application.DTO.Requests.Posts;

public record GetPostRequest
{
    public required Guid Id { get; init; }
}