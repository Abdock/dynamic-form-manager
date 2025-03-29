using System.Text.Json.Nodes;

namespace Application.DTO.Requests.Posts;

public record CreatePostRequest
{
    public required JsonNode Node { get; init; }
}