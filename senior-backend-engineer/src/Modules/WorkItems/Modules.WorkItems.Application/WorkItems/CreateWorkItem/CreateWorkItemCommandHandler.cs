using MediatR;
using Modules.User.Contracts;
using Modules.WorkItems.Application.Abstractions;
using WorkItemAggregate = Modules.WorkItems.Domain.WorkItems.WorkItem;

namespace Modules.WorkItems.Application.WorkItems.CreateWorkItem;

public sealed class CreateWorkItemCommandHandler(
    ISender sender,
    IWorkItemRepository workItemRepository)
    : IRequestHandler<CreateWorkItemCommand, CreateWorkItemResult>
{
    public async Task<CreateWorkItemResult> Handle(
        CreateWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var userExists = await sender.Send(
            new UserExistsQuery(request.AssigneeId),
            cancellationToken);

        if (!userExists)
        {
            return CreateWorkItemResult.UserNotFound();
        }

        var workItem = WorkItemAggregate.Create(
            request.Name,
            request.Description,
            request.AssigneeId);

        await workItemRepository.AddAsync(workItem, cancellationToken);

        return CreateWorkItemResult.Created(new WorkItemDto(
            workItem.Id,
            workItem.Name,
            workItem.Description,
            workItem.AssigneeId));
    }
}
