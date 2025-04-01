using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTO.Responses.Auth;
using Application.Services.Jwt.Constants;
using Application.Services.Jwt.Models;
using Application.Services.Jwt.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Jwt;

public class JwtProvider : IJwtProvider
{
    private readonly IOptions<JwtOptions> _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options;
    }

    public AuthTokenResponse CreateAuthToken(UserClaims claims) => new()
    {
        AccessToken = GenerateAccessToken(claims)
    };

    private string GenerateAccessToken(UserClaims userClaims)
    {
        var options = _options.Value;
        var notBefore = DateTime.UtcNow;
        var expires = notBefore.Add(options.AccessTokenLifetime);
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, userClaims.Id.ToString()),
            new(ClaimTypes.Email, userClaims.Email),
            new(ClaimTypes.GivenName, userClaims.Username),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecurityKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(options.Issuer, options.Audience, claims, notBefore, expires, credentials);
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}