using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Application.DTO.Responses.Posts;

public record PostResponse
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required JsonNode Node { get; init; }

    [JsonIgnore]
    public required string SearchText { get; init; }
}