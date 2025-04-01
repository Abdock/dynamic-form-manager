namespace Application.DTO.Requests.General;

public record PaginationRequest
{
    public required int Skip { get; init; }
    public required int Take { get; init; }

    public static readonly PaginationRequest Default = new()
    {
        Skip = 0,
        Take = 50
    };
}