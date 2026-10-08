using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.User.Domain.Users;
using UserAggregate = Modules.User.Domain.Users.User;

namespace Modules.User.Infrastructure.Persistence;

internal sealed class UserConfiguration : IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).ValueGeneratedNever();

        builder.OwnsOne(
            user => user.Username,
            username =>
            {
                username.Property(value => value.Value)
                    .HasMaxLength(Username.MaxLength)
                    .IsRequired();

                username.Property(value => value.NormalizedValue)
                    .HasMaxLength(Username.MaxLength)
                    .IsRequired();
            });

        builder.Navigation(user => user.Username).IsRequired();
    }
}
