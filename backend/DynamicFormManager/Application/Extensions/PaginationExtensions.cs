using Application.DTO.Responses.General;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions;

public static class PaginationExtensions
{
    public static async ValueTask<PagedResponse<TData>> ToPaged<TData>(IQueryable<TData> data)
    {
        return new PagedResponse<TData>
        {
            Data = await data.ToArrayAsync(),
            Total = await data.CountAsync()
        };
    }
}