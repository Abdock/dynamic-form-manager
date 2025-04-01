using System.Linq.Expressions;
using Application.DTO.Responses.Posts;
using Persistence.Entities;

namespace Application.DTO.Mapping;

public static class PostMapping
{
    public static readonly Expression<Func<Post, PostResponse>> MapToResponseQuery = post => new PostResponse
    {
        Id = post.Id,
        CreatedAt = post.CreatedAt,
        Node = post.Node,
        SearchText = post.SearchText
    };

    public static PostResponse MapToResponse(this Post post) => new()
    {
        Id = post.Id,
        Node = post.Node,
        CreatedAt = post.CreatedAt,
        SearchText = post.SearchText
    };
}