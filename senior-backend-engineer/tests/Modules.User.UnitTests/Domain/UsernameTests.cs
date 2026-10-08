using Modules.User.Domain.Users;
using NUnit.Framework;

namespace Modules.User.UnitTests.Domain;

public sealed class UsernameTests
{
    [Test]
    public void Create_TrimsAndNormalizesUsername()
    {
        var username = Username.Create("  Alice  ");

        Assert.That(username.Value, Is.EqualTo("Alice"));
        Assert.That(username.NormalizedValue, Is.EqualTo("ALICE"));
        Assert.That(username, Is.EqualTo(Username.Create("alice")));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Create_RejectsBlankUsername(string? value)
    {
        Assert.That(() => Username.Create(value), Throws.ArgumentException);
    }

    [Test]
    public void Create_RejectsOverlongUsername()
    {
        var value = new string('a', Username.MaxLength + 1);

        Assert.That(() => Username.Create(value), Throws.ArgumentException);
    }

    [Test]
    public void UserCreate_GeneratesIdentityAndRetainsUsername()
    {
        var username = Username.Create("Alice");

        var user = global::Modules.User.Domain.Users.User.Create(username);

        Assert.That(user.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(user.Username, Is.SameAs(username));
    }
}
