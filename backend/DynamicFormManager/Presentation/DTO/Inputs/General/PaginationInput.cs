namespace Presentation.DTO.Inputs.General;

public record PaginationInput
{
    public int Skip { get; init; } = 0;
    public int Take { get; init; } = 50;
}