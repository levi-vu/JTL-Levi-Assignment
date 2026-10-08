using Modules.WorkItems.Application.WorkItems;

namespace Modules.WorkItems.Application.Abstractions;

public interface IWorkItemQueries
{
    Task<IReadOnlyList<WorkItemDto>> GetByAssigneeAsync(
        Guid assigneeId,
        CancellationToken cancellationToken);
}
