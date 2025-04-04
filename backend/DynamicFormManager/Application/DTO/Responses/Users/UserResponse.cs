﻿namespace Application.DTO.Responses.Users;

public record UserResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
}