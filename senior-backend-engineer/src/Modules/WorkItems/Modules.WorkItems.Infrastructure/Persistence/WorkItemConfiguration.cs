using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkItemAggregate = Modules.WorkItems.Domain.WorkItems.WorkItem;

namespace Modules.WorkItems.Infrastructure.Persistence;

internal sealed class WorkItemConfiguration : IEntityTypeConfiguration<WorkItemAggregate>
{
    public void Configure(EntityTypeBuilder<WorkItemAggregate> builder)
    {
        builder.HasKey(workItem => workItem.Id);
        builder.Property(workItem => workItem.Id).ValueGeneratedNever();
        builder.Property(workItem => workItem.Name).IsRequired();
        builder.Property(workItem => workItem.Description).IsRequired();
        builder.Property(workItem => workItem.AssigneeId).IsRequired();
        builder.HasIndex(workItem => workItem.AssigneeId);
    }
}
