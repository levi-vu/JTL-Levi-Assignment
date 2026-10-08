using FluentValidation;
using Modules.User.Domain.Users;

namespace Modules.User.Application.Users.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Username)
            .Must(username => !string.IsNullOrWhiteSpace(username))
            .WithMessage("Username must not be empty.")
            .Must(username => username is null || username.Trim().Length <= Username.MaxLength)
            .WithMessage($"Username must not exceed {Username.MaxLength} characters.");
    }
}
