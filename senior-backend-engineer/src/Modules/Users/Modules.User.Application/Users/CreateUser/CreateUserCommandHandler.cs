using MediatR;
using Modules.User.Application.Abstractions;
using Modules.User.Domain.Users;

namespace Modules.User.Application.Users.CreateUser;

public sealed class CreateUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = Domain.Users.User.Create(
            Username.Create(request.Username));

        if (!await userRepository.TryAddAsync(user, cancellationToken))
        {
            return CreateUserResult.UsernameConflict();
        }

        return CreateUserResult.Created(new UserDto(user.Id, user.Username.Value));
    }
}