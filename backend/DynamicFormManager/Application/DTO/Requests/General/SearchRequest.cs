namespace Application.DTO.Requests.General;

public record SearchRequest
{
    public required string SearchQuery { get; init; }
}