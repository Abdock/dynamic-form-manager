using Application.CQRS.Commands.Auth;
using Application.CQRS.Queries.User;
using Application.DTO.Responses.Auth;
using Application.DTO.Responses.Users;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO.Inputs.Auth;
using Presentation.DTO.Mapping;
using Presentation.Extensions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthTokenResponse>> Login([FromBody] LoginInput input)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var command = new LoginCommand
        {
            Request = input.MapToRequest()
        };
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<AuthTokenResponse>> Register([FromBody] RegisterInput input)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var command = new RegisterCommand
        {
            Request = input.MapToRequest()
        };
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    [Route("me")]
    public async Task<ActionResult<UserResponse>> Me()
    {
        var cancellationToken = HttpContext.RequestAborted;
        var command = new GetUserInfoQuery
        {
            Request = User.GetUser()
        };
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}