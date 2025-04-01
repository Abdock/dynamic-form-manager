using Application.DTO.Enums;
using Application.DTO.Requests.Auth;
using Application.DTO.Responses.Auth;
using Application.DTO.Responses.General;
using Application.Services.Hasher;
using Application.Services.Jwt;
using Application.Services.Jwt.Models;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.CQRS.Commands.Auth;

public class LoginCommand : ICommand<BaseResponse<AuthTokenResponse>>
{
    public required LoginRequest Request { get; init; }
}

public class LoginCommandHandler : ICommandHandler<LoginCommand, BaseResponse<AuthTokenResponse>>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IDbContextFactory<FormContext> contextFactory, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _contextFactory = contextFactory;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async ValueTask<BaseResponse<AuthTokenResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Email == command.Request.Email, cancellationToken);
        if (user is null)
        {
            return CustomStatusCodes.InvalidUserCredentials;
        }

        var isPasswordVerified = _passwordHasher.Verify(command.Request.Password, user.PasswordHash);
        if (!isPasswordVerified)
        {
            return CustomStatusCodes.InvalidUserCredentials;
        }

        var claims = new UserClaims
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
        };
        return _jwtProvider.CreateAuthToken(claims);
    }
}