using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Modules.User.Domain.Users;
using Modules.User.Infrastructure.Persistence;
using NUnit.Framework;
using UserAggregate = Modules.User.Domain.Users.User;

namespace Modules.User.UnitTests.Infrastructure;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public sealed class UserRepositoryTests
{
    private readonly string _databaseName;
    private readonly InMemoryDatabaseRoot _databaseRoot;

    public UserRepositoryTests()
    {
        _databaseName = Guid.NewGuid().ToString();
        _databaseRoot = new InMemoryDatabaseRoot();
    }

    [Test]
    public async Task TryAddAsync_RejectsCaseInsensitiveDuplicate()
    {
        await using var dbContext = CreateDbContext();
        var repository = new UserRepository(dbContext);

        var firstAdded = await repository.TryAddAsync(
            UserAggregate.Create(Username.Create("Alice")),
            CancellationToken.None);
        var duplicateAdded = await repository.TryAddAsync(
            UserAggregate.Create(Username.Create("ALICE")),
            CancellationToken.None);

        Assert.That(firstAdded, Is.True);
        Assert.That(duplicateAdded, Is.False);
        Assert.That(await dbContext.Users.CountAsync(), Is.EqualTo(1));
    }

    private UsersDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseInMemoryDatabase(_databaseName, _databaseRoot)
            .Options;

        return new UsersDbContext(options);
    }
}