using Application.DTO.Responses.Auth;
using Application.Services.Jwt.Models;

namespace Application.Services.Jwt;

public interface IJwtProvider
{
    AuthTokenResponse CreateAuthToken(UserClaims claims);
}