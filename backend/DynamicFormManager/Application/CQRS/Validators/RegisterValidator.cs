using Application.Constants;
using Application.CQRS.Commands.Auth;
using Application.DTO.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.CQRS.Validators;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator(IDbContextFactory<FormContext> contextFactory)
    {
        RuleFor(e => e.Request.Email)
            .EmailAddress()
            .WithErrorCode(CustomStatusCodes.InvalidEmailFormat.ToString())
            .MustAsync(async (email, cancellationToken) =>
            {
                await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
                return await context.Users.AllAsync(e => e.Email != email, cancellationToken);
            })
            .WithErrorCode(CustomStatusCodes.EmailAlreadyUsing.ToString());
        RuleFor(e => e.Request.Password)
            .Matches(ValidationRegexes.PasswordRegex())
            .WithErrorCode(CustomStatusCodes.WeakPassword.ToString());
    }
}