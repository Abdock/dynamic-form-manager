using Application.DTO.Requests.Auth;
using Application.DTO.Responses.Auth;
using Application.DTO.Responses.General;
using Application.Services.Hasher;
using Application.Services.Jwt;
using Application.Services.Jwt.Models;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities;

namespace Application.CQRS.Commands.Auth;

public class RegisterCommand : ICommand<BaseResponse<AuthTokenResponse>>
{
    public required RegisterRequest Request { get; init; }
}

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, BaseResponse<AuthTokenResponse>>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IDbContextFactory<FormContext> contextFactory, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _contextFactory = contextFactory;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<BaseResponse<AuthTokenResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var passwordHash = _passwordHasher.ComputeHash(command.Request.Password);
        var user = new User
        {
            Email = command.Request.Email,
            PasswordHash = passwordHash,
            Username = command.Request.Username
        };
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        var claims = new UserClaims
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username
        };
        var token = _jwtProvider.CreateAuthToken(claims);
        return token;
    }
}