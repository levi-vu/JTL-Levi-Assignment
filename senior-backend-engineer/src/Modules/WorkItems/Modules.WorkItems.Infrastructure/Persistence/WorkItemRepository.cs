using Modules.WorkItems.Application.Abstractions;
using Modules.WorkItems.Domain.WorkItems;

namespace Modules.WorkItems.Infrastructure.Persistence;

public sealed class WorkItemRepository(WorkItemsDbContext dbContext) : IWorkItemRepository
{
    public async Task AddAsync(WorkItem workItem, CancellationToken cancellationToken)
    {
        dbContext.WorkItems.Add(workItem);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
