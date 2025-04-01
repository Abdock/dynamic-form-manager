using Application.CQRS.Commands.Posts;
using Application.CQRS.Queries.Posts;
using Application.DTO.Requests.Posts;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Posts;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO.Inputs.Posts;
using Presentation.DTO.Mapping;
using Presentation.Extensions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<PostResponse>>> Posts([FromQuery] GetPagedPostsInput input)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var request = input.MapToRequest();
        var query = new GetPagedPostsQuery
        {
            Request = request
        };
        var response = await _mediator.Send(query, cancellationToken);
        return response;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<BaseResponse<PostResponse>>> Create([FromBody] CreatePostInput input)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var request = input.MapToRequest();
        var command = new CreatePostCommand
        {
            Request = request,
            User = User.GetUser()
        };
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = response.Response!.Id }, response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<BaseResponse<PostResponse>>> Get([FromRoute] Guid id)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var request = new GetPostRequest
        {
            Id = id
        };
        var query = new GetPostQuery
        {
            Request = request
        };
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }
}