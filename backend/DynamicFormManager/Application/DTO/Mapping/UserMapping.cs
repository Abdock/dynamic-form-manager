using System.Linq.Expressions;
using Application.DTO.Responses.Users;
using Persistence.Entities;

namespace Application.DTO.Mapping;

public static class UserMapping
{
    public static readonly Expression<Func<User, UserResponse>> MapToResponseQuery = user => new UserResponse
    {
        Id = user.Id,
        Email = user.Email,
        Username = user.Username
    };
}