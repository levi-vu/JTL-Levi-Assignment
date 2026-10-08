using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Modules.User.Application.Users.GetUser;

namespace Modules.User.Api.Endpoints;

public sealed record GetUserByIdRequest(Guid Id);

public sealed class GetUserByIdEndpoint(ISender sender)
    : Endpoint<GetUserByIdRequest, UserResponse>
{
    public override void Configure()
    {
        Get("/users/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetUserByIdRequest request,
        CancellationToken cancellationToken)
    {
        var user = await sender.Send(new GetUserByIdQuery(request.Id), cancellationToken);

        if (user is null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        await SendAsync(
            new UserResponse(user.Id, user.Username),
            cancellation: cancellationToken);
    }
}
