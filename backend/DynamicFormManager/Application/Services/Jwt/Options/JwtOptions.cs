﻿namespace Application.Services.Jwt.Options;

public record JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required TimeSpan AccessTokenLifetime { get; init; }
    public required string SecurityKey { get; init; }
}