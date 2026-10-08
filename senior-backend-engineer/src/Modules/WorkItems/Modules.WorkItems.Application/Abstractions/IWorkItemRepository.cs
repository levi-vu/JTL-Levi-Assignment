using Modules.WorkItems.Domain.WorkItems;

namespace Modules.WorkItems.Application.Abstractions;

public interface IWorkItemRepository
{
    Task AddAsync(WorkItem workItem, CancellationToken cancellationToken);
}
