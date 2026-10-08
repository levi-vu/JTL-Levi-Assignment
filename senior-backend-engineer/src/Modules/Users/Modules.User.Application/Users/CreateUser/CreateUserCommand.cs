using MediatR;

namespace Modules.User.Application.Users.CreateUser;

public sealed record CreateUserCommand(string? Username) : IRequest<CreateUserResult>;

public sealed record CreateUserResult(CreateUserStatus Status, UserDto? User)
{
    public static CreateUserResult Created(UserDto user) =>
        new(CreateUserStatus.Created, user);

    public static CreateUserResult UsernameConflict() =>
        new(CreateUserStatus.UsernameConflict, null);
}

public enum CreateUserStatus
{
    Created,
    UsernameConflict
}
