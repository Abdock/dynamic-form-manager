using Application.CQRS.Commands.Auth;
using Application.CQRS.Queries.Posts;
using Application.CQRS.Queries.User;
using Application.DTO.Requests.Posts;
using Application.DTO.Responses.Auth;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Posts;
using Application.DTO.Responses.Users;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO.Inputs.Auth;
using Presentation.DTO.Inputs.Posts;
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

    [HttpGet]
    [Route("{id:guid}/posts")]
    public async Task<ActionResult<PagedResponse<PostResponse>>> UserPosts([FromRoute] Guid id, [FromQuery] GetPagedPostsInput input)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var request = new GetUserPostsRequest
        {
            UserId = id,
            Query = input.MapToRequest()
        };
        var query = new GetUserPagedPostsQuery
        {
            Request = request
        };
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
}