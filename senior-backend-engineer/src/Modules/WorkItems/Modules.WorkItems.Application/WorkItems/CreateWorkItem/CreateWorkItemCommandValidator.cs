using FluentValidation;

namespace Modules.WorkItems.Application.WorkItems.CreateWorkItem;

public sealed class CreateWorkItemCommandValidator : AbstractValidator<CreateWorkItemCommand>
{
    public CreateWorkItemCommandValidator()
    {
        RuleFor(command => command.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Work item name must not be empty.");

        RuleFor(command => command.AssigneeId)
            .NotEmpty()
            .WithMessage("Assignee ID must not be empty.");
    }
}
