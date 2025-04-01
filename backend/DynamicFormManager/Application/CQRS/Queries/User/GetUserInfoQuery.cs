using Application.DTO.Mapping;
using Application.DTO.Requests.Auth;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Users;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.CQRS.Queries.User;

public class GetUserInfoQuery : IQuery<BaseResponse<UserResponse>>
{
    public required AuthorizedUserRequest Request { get; init; }
}

public class GetUserInfoQueryHandler : IQueryHandler<GetUserInfoQuery, BaseResponse<UserResponse>>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;

    public GetUserInfoQueryHandler(IDbContextFactory<FormContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async ValueTask<BaseResponse<UserResponse>> Handle(GetUserInfoQuery query, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var user = await context.Users.Where(e => e.Id == query.Request.UserId)
            .AsNoTracking()
            .Select(UserMapping.MapToResponseQuery)
            .FirstAsync(cancellationToken);
        return user;
    }
}