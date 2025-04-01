using Application.DTO.Requests.Auth;
using Presentation.DTO.Inputs;

namespace Presentation.DTO.Mapping;

public static class AuthMapping
{
    public static LoginRequest MapToRequest(this LoginInput input) => new()
    {
        Email = input.Email.Trim().ToLower(),
        Password = input.Password
    };

    public static RegisterRequest MapToRequest(this RegisterInput input) => new()
    {
        Email = input.Email,
        Password = input.Password,
        Username = input.Username
    };
}