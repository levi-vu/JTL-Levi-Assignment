using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Modules.User.Application.Users.CreateUser;

namespace Modules.User.Api.Endpoints;

public sealed record CreateUserRequest(string? Username);

public sealed record UserResponse(Guid Id, string Username);

public sealed class CreateUserEndpoint(
    ISender sender,
    IValidator<CreateUserCommand> validator)
    : Endpoint<CreateUserRequest, UserResponse>
{
    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(request.Username);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            foreach (var failure in validationResult.Errors)
            {
                AddError(failure);
            }

            await SendErrorsAsync(cancellation: cancellationToken);
            return;
        }

        var result = await sender.Send(command, cancellationToken);

        if (result.Status == CreateUserStatus.UsernameConflict)
        {
            AddError("Username is already registered.");
            await SendErrorsAsync(StatusCodes.Status409Conflict, cancellationToken);
            return;
        }

        var user = result.User!;
        HttpContext.Response.Headers.Location = $"/users/{user.Id}";
        await SendAsync(
            new UserResponse(user.Id, user.Username),
            StatusCodes.Status201Created,
            cancellationToken);
    }
}
