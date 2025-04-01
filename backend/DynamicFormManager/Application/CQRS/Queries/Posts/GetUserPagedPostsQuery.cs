using Application.DTO.Mapping;
using Application.DTO.Requests.Auth;
using Application.DTO.Requests.General;
using Application.DTO.Requests.Posts;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Posts;
using Application.Extensions;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.CQRS.Queries.Posts;

public class GetUserPagedPostsQuery : IQuery<PagedResponse<PostResponse>>
{
    public required GetUserPostsRequest Request { get; init; }
}

public class GetUserPagedPostsQueryHandler : IQueryHandler<GetUserPagedPostsQuery, PagedResponse<PostResponse>>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;

    public GetUserPagedPostsQueryHandler(IDbContextFactory<FormContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async ValueTask<PagedResponse<PostResponse>> Handle(GetUserPagedPostsQuery query, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var response = await context.Posts
            .Where(e => e.UserId == query.Request.UserId)
            .AsNoTracking()
            .Select(PostMapping.MapToResponseQuery)
            .Search(query.Request.Query)
            .ApplyPaginationAsync(query.Request.Query, cancellationToken);
        return response;
    }
}