namespace Application.DTO.Responses.General;

public record PagedResponse<TData>
{
    public required IReadOnlyCollection<TData> Data { get; init; }
    public required int Total { get; init; }
}