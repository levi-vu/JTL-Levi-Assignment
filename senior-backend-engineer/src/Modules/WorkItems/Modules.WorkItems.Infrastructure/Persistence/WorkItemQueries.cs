using Microsoft.EntityFrameworkCore;
using Modules.WorkItems.Application.Abstractions;
using Modules.WorkItems.Application.WorkItems;

namespace Modules.WorkItems.Infrastructure.Persistence;

public sealed class WorkItemQueries(WorkItemsDbContext dbContext) : IWorkItemQueries
{
    public async Task<IReadOnlyList<WorkItemDto>> GetByAssigneeAsync(
        Guid assigneeId,
        CancellationToken cancellationToken) =>
        await dbContext.WorkItems
            .AsNoTracking()
            .Where(workItem => workItem.AssigneeId == assigneeId)
            .Select(workItem => new WorkItemDto(
                workItem.Id,
                workItem.Name,
                workItem.Description,
                workItem.AssigneeId))
            .ToListAsync(cancellationToken);
}
