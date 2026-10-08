using MediatR;
using Modules.User.Application.Abstractions;

namespace Modules.User.Application.Users.GetUser;

public sealed class GetUserByIdQueryHandler(IUserQueries userQueries)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public Task<UserDto?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken) =>
        userQueries.GetByIdAsync(request.UserId, cancellationToken);
}
