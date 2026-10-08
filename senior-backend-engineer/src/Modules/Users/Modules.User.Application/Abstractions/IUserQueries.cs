using Modules.User.Application.Users;

namespace Modules.User.Application.Abstractions;

public interface IUserQueries
{
    Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken);
}
