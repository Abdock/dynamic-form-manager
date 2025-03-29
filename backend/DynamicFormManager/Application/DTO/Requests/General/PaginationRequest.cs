namespace Application.DTO.Requests.General;

public record PaginationRequest
{
    public required int Skip { get; init; }
    public required int Limit { get; init; }

    public static readonly PaginationRequest Empty = new()
    {
        Skip = 0,
        Limit = 50
    };
}