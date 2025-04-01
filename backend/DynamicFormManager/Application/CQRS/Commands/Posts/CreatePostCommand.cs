using Application.DTO.Mapping;
using Application.DTO.Requests.Auth;
using Application.DTO.Requests.Posts;
using Application.DTO.Responses.General;
using Application.DTO.Responses.Posts;
using Application.Services.SearchText;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities;

namespace Application.CQRS.Commands.Posts;

public class CreatePostCommand : ICommand<BaseResponse<PostResponse>>
{
    public required CreatePostRequest Request { get; init; }
    public required AuthorizedUserRequest User { get; init; }
}

public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, BaseResponse<PostResponse>>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;
    private readonly ISearchTextProvider _searchTextProvider;

    public CreatePostCommandHandler(IDbContextFactory<FormContext> contextFactory, ISearchTextProvider searchTextProvider)
    {
        _contextFactory = contextFactory;
        _searchTextProvider = searchTextProvider;
    }

    public async ValueTask<BaseResponse<PostResponse>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var searchText = _searchTextProvider.GetSearchText(command.Request.Node);
        var post = new Post
        {
            Node = command.Request.Node,
            SearchText = searchText,
            UserId = command.User.UserId
        };
        await context.Posts.AddAsync(post, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return post.MapToResponse();
    }
}