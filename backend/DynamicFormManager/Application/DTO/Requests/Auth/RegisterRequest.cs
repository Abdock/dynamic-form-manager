﻿namespace Application.DTO.Requests.Auth;

public record RegisterRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Username { get; init; }
}