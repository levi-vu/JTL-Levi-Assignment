namespace Modules.WorkItems.Domain.WorkItems;

public sealed class WorkItem
{
    private WorkItem()
    {
    }

    private WorkItem(Guid id, string name, string description, Guid assigneeId)
    {
        Id = id;
        Name = name;
        Description = description;
        AssigneeId = assigneeId;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public Guid AssigneeId { get; private set; }

    public static WorkItem Create(string? name, string? description, Guid assigneeId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Work item name must not be empty.", nameof(name));
        }

        if (assigneeId == Guid.Empty)
        {
            throw new ArgumentException("Assignee ID must not be empty.", nameof(assigneeId));
        }

        return new WorkItem(
            Guid.NewGuid(),
            name.Trim(),
            description?.Trim() ?? string.Empty,
            assigneeId);
    }
}
