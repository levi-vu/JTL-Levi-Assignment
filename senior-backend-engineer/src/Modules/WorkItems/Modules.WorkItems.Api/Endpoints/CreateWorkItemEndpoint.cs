using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Modules.WorkItems.Application.WorkItems.CreateWorkItem;

namespace Modules.WorkItems.Api.Endpoints;

public sealed record CreateWorkItemRequest(
    string? Name,
    string? Description,
    Guid AssigneeId);

public sealed record WorkItemResponse(
    Guid Id,
    string Name,
    string Description,
    Guid AssigneeId);

public sealed class CreateWorkItemEndpoint(
    ISender sender,
    IValidator<CreateWorkItemCommand> validator)
    : Endpoint<CreateWorkItemRequest, WorkItemResponse>
{
    public override void Configure()
    {
        Post("/work-items");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        CreateWorkItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateWorkItemCommand(
            request.Name,
            request.Description,
            request.AssigneeId);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            foreach (var failure in validationResult.Errors)
            {
                AddError(failure);
            }

            await SendErrorsAsync(cancellation: cancellationToken);
            return;
        }

        var result = await sender.Send(command, cancellationToken);

        if (result.Status == CreateWorkItemStatus.UserNotFound)
        {
            AddError("User not found");
            await SendErrorsAsync(StatusCodes.Status404NotFound, cancellationToken);
            return;
        }

        var workItem = result.WorkItem!;
        await SendAsync(
            new WorkItemResponse(
                workItem.Id,
                workItem.Name,
                workItem.Description,
                workItem.AssigneeId),
            StatusCodes.Status201Created,
            cancellationToken);
    }
}
