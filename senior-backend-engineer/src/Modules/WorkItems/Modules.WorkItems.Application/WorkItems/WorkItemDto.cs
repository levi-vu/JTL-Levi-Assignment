namespace Modules.WorkItems.Application.WorkItems;

public sealed record WorkItemDto(
    Guid Id,
    string Name,
    string Description,
    Guid AssigneeId);
