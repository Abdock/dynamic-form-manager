using FluentValidation;
using Mediator;

namespace Application.CQRS.Pipelines;

public class ValidationPipeline<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse> where TMessage : IMessage
{
    private readonly IEnumerable<IValidator<TMessage>> _validators;

    public ValidationPipeline(IEnumerable<IValidator<TMessage>> validators)
    {
        _validators = validators;
    }

    public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
    {
        foreach (var validator in _validators)
        {
            await validator.ValidateAndThrowAsync(message, cancellationToken);
        }

        return await next(message, cancellationToken);
    }
}