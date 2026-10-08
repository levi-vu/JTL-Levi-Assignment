using Microsoft.EntityFrameworkCore;
using Modules.User.Application.Abstractions;
using Modules.User.Application.Users;

namespace Modules.User.Infrastructure.Persistence;

public sealed class UserQueries(UsersDbContext dbContext) : IUserQueries
{
    public Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken) =>
        dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new UserDto(user.Id, user.Username.Value))
            .SingleOrDefaultAsync(cancellationToken);

    public Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken) =>
        dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Id == userId, cancellationToken);
}
