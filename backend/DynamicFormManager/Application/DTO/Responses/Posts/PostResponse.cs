using System.Text.Json.Nodes;

namespace Application.DTO.Responses.Posts;

public record PostResponse
{
    public required Guid Id { get; init; }
    public required JsonNode Node { get; init; }
}