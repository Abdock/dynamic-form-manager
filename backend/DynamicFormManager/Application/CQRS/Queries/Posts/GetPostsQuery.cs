using Application.DTO.Requests.General;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Posts;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.CQRS.Queries.Posts;

public class GetPostsQuery : IQuery<PagedResponse<PostResponse>>
{
    public required QueryRequest Request { get; init; }
}

public class GetPostsQueryHandler : IQueryHandler<GetPostsQuery, PagedResponse<PostResponse>>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;

    public GetPostsQueryHandler(IDbContextFactory<FormContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async ValueTask<PagedResponse<PostResponse>> Handle(GetPostsQuery query, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var postsQuery = context.Posts
            .Select(e=>new PostResponse
            {
                Id = e.Id,
                Node = e.Node
            })
            .AsNoTracking();
        var response = await query.Request
            .ApplyQueryAsync(postsQuery);
        return response;
    }
}