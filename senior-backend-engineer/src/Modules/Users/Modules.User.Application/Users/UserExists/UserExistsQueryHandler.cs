using MediatR;
using Modules.User.Application.Abstractions;
using Modules.User.Contracts;

namespace Modules.User.Application.Users.UserExists;

public sealed class UserExistsQueryHandler(IUserQueries userQueries)
    : IRequestHandler<UserExistsQuery, bool>
{
    public Task<bool> Handle(
        UserExistsQuery request,
        CancellationToken cancellationToken) =>
        userQueries.ExistsAsync(request.UserId, cancellationToken);
}
