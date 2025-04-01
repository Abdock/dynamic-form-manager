using System.Text.Json.Nodes;

namespace Presentation.DTO.Inputs.Posts;

public record CreatePostInput
{
    public required JsonNode Node { get; init; }
}