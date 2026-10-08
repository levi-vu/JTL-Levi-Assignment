using MediatR;

namespace Modules.WorkItems.Application.WorkItems.GetWorkItemsByAssignee;

public sealed record GetWorkItemsByAssigneeQuery(Guid AssigneeId)
    : IRequest<IReadOnlyList<WorkItemDto>>;
