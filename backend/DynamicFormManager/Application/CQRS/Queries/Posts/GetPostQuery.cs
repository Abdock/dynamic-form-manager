using Application.DTO.Mapping;
using Application.DTO.Requests.Posts;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Posts;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.CQRS.Queries.Posts;

public class GetPostQuery : IQuery<BaseResponse<PostResponse>>
{
    public required GetPostRequest Request { get; init; }
}

public class GetPostQueryHandler : IQueryHandler<GetPostQuery, BaseResponse<PostResponse>>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;

    public GetPostQueryHandler(IDbContextFactory<FormContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async ValueTask<BaseResponse<PostResponse>> Handle(GetPostQuery query, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var post = await context.Posts
            .AsNoTracking()
            .Select(PostMapping.MapToResponseQuery)
            .FirstAsync(e => e.Id == query.Request.Id, cancellationToken);
        return post;
    }
}