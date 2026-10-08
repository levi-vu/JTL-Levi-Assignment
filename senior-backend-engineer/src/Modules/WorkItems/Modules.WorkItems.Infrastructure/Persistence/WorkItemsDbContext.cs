using Microsoft.EntityFrameworkCore;
using WorkItemAggregate = Modules.WorkItems.Domain.WorkItems.WorkItem;

namespace Modules.WorkItems.Infrastructure.Persistence;

public sealed class WorkItemsDbContext(DbContextOptions<WorkItemsDbContext> options)
    : DbContext(options)
{
    public DbSet<WorkItemAggregate> WorkItems => Set<WorkItemAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WorkItemConfiguration());
    }
}
