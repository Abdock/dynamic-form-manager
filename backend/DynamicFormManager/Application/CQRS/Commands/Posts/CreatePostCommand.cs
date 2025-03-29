using Application.DTO.Requests.Posts;
using Application.Services.SearchText;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities;

namespace Application.CQRS.Commands.Posts;

public class CreatePostCommand : ICommand
{
    public required CreatePostRequest Request { get; init; }
}

public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand>
{
    private readonly IDbContextFactory<FormContext> _contextFactory;
    private readonly ISearchTextProvider _searchTextProvider;

    public CreatePostCommandHandler(IDbContextFactory<FormContext> contextFactory, ISearchTextProvider searchTextProvider)
    {
        _contextFactory = contextFactory;
        _searchTextProvider = searchTextProvider;
    }

    public async ValueTask<Unit> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var searchText = _searchTextProvider.GetSearchText(command.Request.Node);
        var post = new Post
        {
            Node = command.Request.Node,
            SearchText = searchText,
        };
        await context.Posts.AddAsync(post, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}