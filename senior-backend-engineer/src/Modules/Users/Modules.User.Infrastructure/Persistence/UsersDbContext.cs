using Microsoft.EntityFrameworkCore;
using UserAggregate = Modules.User.Domain.Users.User;

namespace Modules.User.Infrastructure.Persistence;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<UserAggregate> Users => Set<UserAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
