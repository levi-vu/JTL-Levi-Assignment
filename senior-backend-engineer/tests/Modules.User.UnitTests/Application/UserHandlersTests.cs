using Modules.User.Application.Abstractions;
using Modules.User.Application.Users;
using Modules.User.Application.Users.CreateUser;
using Modules.User.Application.Users.GetUser;
using Modules.User.Application.Users.UserExists;
using Modules.User.Contracts;
using Modules.User.Domain.Users;
using NSubstitute;
using NUnit.Framework;
using UserAggregate = Modules.User.Domain.Users.User;

namespace Modules.User.UnitTests.Application;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public sealed class UserHandlersTests
{
    private readonly IUserRepository _repository;
    private readonly IUserQueries _queries;

    public UserHandlersTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _queries = Substitute.For<IUserQueries>();
    }

    [Test]
    public async Task CreateUser_ReturnsCreatedUser_WhenUsernameIsAvailable()
    {
        _repository.TryAddAsync(Arg.Any<UserAggregate>(), Arg.Any<CancellationToken>())
            .Returns(true);
        var handler = new CreateUserCommandHandler(_repository);

        var result = await handler.Handle(new CreateUserCommand(" Alice "), CancellationToken.None);

        Assert.That(result.Status, Is.EqualTo(CreateUserStatus.Created));
        Assert.That(result.User?.Username, Is.EqualTo("Alice"));
        await _repository.Received(1).TryAddAsync(
            Arg.Is<UserAggregate>(user => user.Username.Value == "Alice"),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task CreateUser_ReturnsConflict_WhenUsernameAlreadyExists()
    {
        _repository.TryAddAsync(Arg.Any<UserAggregate>(), Arg.Any<CancellationToken>())
            .Returns(false);
        var handler = new CreateUserCommandHandler(_repository);

        var result = await handler.Handle(new CreateUserCommand("ALICE"), CancellationToken.None);

        Assert.That(result.Status, Is.EqualTo(CreateUserStatus.UsernameConflict));
        Assert.That(result.User, Is.Null);
    }

    [Test]
    public async Task CreateUserValidator_RejectsBlankAndOverlongUsernames()
    {
        var validator = new CreateUserCommandValidator();

        var blank = await validator.ValidateAsync(new CreateUserCommand("   "));
        var overlong = await validator.ValidateAsync(
            new CreateUserCommand(new string('a', Username.MaxLength + 1)));

        Assert.That(blank.IsValid, Is.False);
        Assert.That(overlong.IsValid, Is.False);
    }

    [Test]
    public async Task GetUser_ReturnsProjectedUser_WhenFound()
    {
        var userId = Guid.NewGuid();
        _queries.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new UserDto(userId, "Alice"));
        var handler = new GetUserByIdQueryHandler(_queries);

        var result = await handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);

        Assert.That(result, Is.EqualTo(new UserDto(userId, "Alice")));
    }

    [Test]
    public async Task GetUser_ReturnsNull_WhenMissing()
    {
        _queries.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((UserDto?)null);
        var handler = new GetUserByIdQueryHandler(_queries);

        var result = await handler.Handle(
            new GetUserByIdQuery(Guid.NewGuid()),
            CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task UserExists_ReturnsReadModelResult(bool exists)
    {
        var userId = Guid.NewGuid();
        _queries.ExistsAsync(userId, Arg.Any<CancellationToken>()).Returns(exists);
        var handler = new UserExistsQueryHandler(_queries);

        var result = await handler.Handle(new UserExistsQuery(userId), CancellationToken.None);

        Assert.That(result, Is.EqualTo(exists));
    }
}
