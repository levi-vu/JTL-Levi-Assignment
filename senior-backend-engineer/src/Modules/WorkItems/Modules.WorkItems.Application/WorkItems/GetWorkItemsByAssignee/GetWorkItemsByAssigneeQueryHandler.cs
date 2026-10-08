using MediatR;
using Modules.WorkItems.Application.Abstractions;

namespace Modules.WorkItems.Application.WorkItems.GetWorkItemsByAssignee;

public sealed class GetWorkItemsByAssigneeQueryHandler(IWorkItemQueries workItemQueries)
    : IRequestHandler<GetWorkItemsByAssigneeQuery, IReadOnlyList<WorkItemDto>>
{
    public Task<IReadOnlyList<WorkItemDto>> Handle(
        GetWorkItemsByAssigneeQuery request,
        CancellationToken cancellationToken) =>
        workItemQueries.GetByAssigneeAsync(request.AssigneeId, cancellationToken);
}
