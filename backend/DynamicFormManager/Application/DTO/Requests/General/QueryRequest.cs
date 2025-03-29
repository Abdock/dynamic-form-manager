using Application.DTO.Responses.General;
using Microsoft.EntityFrameworkCore;

namespace Application.DTO.Requests.General;

public record QueryRequest
{
    public PaginationRequest Pagination { get; init; } = PaginationRequest.Empty;

    public async Task<PagedResponse<TData>> ApplyQueryAsync<TData>(IQueryable<TData> queryable)
    {
        var total = await queryable.CountAsync();
        var data = await queryable.ToArrayAsync();
        return new PagedResponse<TData>
        {
            Data = data,
            Total = total
        };
    }
}