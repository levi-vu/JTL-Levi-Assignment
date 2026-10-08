namespace Modules.User.Application.Abstractions;

public interface IUserRepository
{
    Task<bool> TryAddAsync(
        Domain.Users.User user,
        CancellationToken cancellationToken);
}