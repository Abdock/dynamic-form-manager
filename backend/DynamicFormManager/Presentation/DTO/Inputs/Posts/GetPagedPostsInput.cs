using Presentation.DTO.Inputs.General;

namespace Presentation.DTO.Inputs.Posts;

public record GetPagedPostsInput : PaginationInput
{
    public string SearchText { get; init; } = string.Empty;
}