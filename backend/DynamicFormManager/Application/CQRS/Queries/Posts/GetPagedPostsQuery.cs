﻿using Application.DTO.Mapping;
using Application.DTO.Requests.General;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Posts;
using Application.Extensions;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.CQRS.Queries.Posts;

public class GetPagedPostsQuery : IQuery<PagedResponse<PostResponse>>
{
    public required QueryRequest Request { get; init; }
}

public class GetPagedPostsQueryHandler : IQueryHandler<GetPagedPostsQuery, PagedResponse<PostResponse>>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;

    public GetPagedPostsQueryHandler(IDbContextFactory<FormContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async ValueTask<PagedResponse<PostResponse>> Handle(GetPagedPostsQuery query, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var response = await context.Posts
            .Select(PostMapping.MapToResponseQuery)
            .AsNoTracking()
            .Search(query.Request)
            .ApplyPaginationAsync(query.Request, cancellationToken);
        return response;
    }
}