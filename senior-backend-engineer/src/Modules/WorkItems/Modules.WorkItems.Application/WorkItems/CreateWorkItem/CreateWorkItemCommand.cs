using MediatR;

namespace Modules.WorkItems.Application.WorkItems.CreateWorkItem;

public sealed record CreateWorkItemCommand(
    string? Name,
    string? Description,
    Guid AssigneeId) : IRequest<CreateWorkItemResult>;

public sealed record CreateWorkItemResult(
    CreateWorkItemStatus Status,
    WorkItemDto? WorkItem)
{
    public static CreateWorkItemResult Created(WorkItemDto workItem) =>
        new(CreateWorkItemStatus.Created, workItem);

    public static CreateWorkItemResult UserNotFound() =>
        new(CreateWorkItemStatus.UserNotFound, null);
}

public enum CreateWorkItemStatus
{
    Created,
    UserNotFound
}
