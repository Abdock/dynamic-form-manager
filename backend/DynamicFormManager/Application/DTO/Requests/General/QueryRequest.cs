namespace Application.DTO.Requests.General;

public record QueryRequest
{
    public PaginationRequest Pagination { get; init; } = PaginationRequest.Default;
    public SearchRequest? Search { get; init; }
}