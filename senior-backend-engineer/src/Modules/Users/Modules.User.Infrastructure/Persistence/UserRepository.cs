using Microsoft.EntityFrameworkCore;
using Modules.User.Application.Abstractions;
using UserAggregate = Modules.User.Domain.Users.User;

namespace Modules.User.Infrastructure.Persistence;

public sealed class UserRepository(UsersDbContext dbContext) : IUserRepository
{
    public async Task<bool> TryAddAsync(
        UserAggregate user,
        CancellationToken cancellationToken)
    {
        var usernameExists = await dbContext.Users.AnyAsync(
            existingUser =>
                existingUser.Username.NormalizedValue == user.Username.NormalizedValue,
            cancellationToken);

        if (usernameExists)
        {
            return false;
        }

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
