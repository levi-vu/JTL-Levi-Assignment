using FastEndpoints;
using MediatR;
using Modules.WorkItems.Application.WorkItems.GetWorkItemsByAssignee;

namespace Modules.WorkItems.Api.Endpoints;

public sealed record GetWorkItemsByAssigneeRequest(Guid AssigneeId);

public sealed class GetWorkItemsByAssigneeEndpoint(ISender sender)
    : Endpoint<GetWorkItemsByAssigneeRequest, IReadOnlyList<WorkItemResponse>>
{
    public override void Configure()
    {
        Get("/work-items");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetWorkItemsByAssigneeRequest request,
        CancellationToken cancellationToken)
    {
        var workItems = await sender.Send(
            new GetWorkItemsByAssigneeQuery(request.AssigneeId),
            cancellationToken);

        await SendAsync(
            workItems
                .Select(workItem => new WorkItemResponse(
                    workItem.Id,
                    workItem.Name,
                    workItem.Description,
                    workItem.AssigneeId))
                .ToArray(),
            cancellation: cancellationToken);
    }
}
