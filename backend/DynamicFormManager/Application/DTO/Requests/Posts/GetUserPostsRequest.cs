using Application.DTO.Requests.General;

namespace Application.DTO.Requests.Posts;

public record GetUserPostsRequest
{
    public required Guid UserId { get; init; }
    public required QueryRequest Query { get; init; }
}