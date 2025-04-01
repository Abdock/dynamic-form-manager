using Application.DTO.Requests.General;
using Application.DTO.Requests.Posts;
using Presentation.DTO.Inputs.Posts;

namespace Presentation.DTO.Mapping;

public static class PostMapping
{
    public static QueryRequest MapToRequest(this GetPagedPostsInput input) => new()
    {
        Pagination = new PaginationRequest
        {
            Skip = input.Skip,
            Take = input.Take
        },
        Search = new SearchRequest
        {
            SearchQuery = input.SearchText
        }
    };

    public static CreatePostRequest MapToRequest(this CreatePostInput input) => new()
    {
        Node = input.Node
    };
}