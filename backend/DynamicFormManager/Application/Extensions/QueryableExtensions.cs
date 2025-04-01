using Application.DTO.Requests.General;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Posts;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Application.Extensions;

public static class QueryableExtensions
{
    public static async ValueTask<PagedResponse<TData>> ApplyPaginationAsync<TData>(this IQueryable<TData> queryable, QueryRequest request, CancellationToken cancellationToken = default)
    {
        var total = await queryable.CountAsync(cancellationToken);
        var data = await queryable
            .Skip(request.Pagination.Skip)
            .Take(request.Pagination.Take)
            .ToArrayAsync(cancellationToken);
        return new PagedResponse<TData>
        {
            Data = data,
            Total = total
        };
    }

    public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> queryable, QueryRequest request) where TEntity : PostResponse
    {
        return request.Search is null ? queryable : queryable.Where(e => e.SearchText.Contains(request.Search.SearchQuery));
    }
}