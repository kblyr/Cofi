using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cofi.Identity.EntityTypeConfigurations;

sealed class UserDomain_ETC : IEntityTypeConfiguration<UserDomain>
{
    public void Configure(EntityTypeBuilder<UserDomain> builder)
    {
        builder.ToTable("UserDomain", DatabaseDefaults.Schema);

        builder.HasOne(userDomain => userDomain.User)
            .WithMany()
            .HasForeignKey(userDomain => userDomain.UserId);

        builder.HasOne(userDomain => userDomain.Domain)
            .WithMany()
            .HasForeignKey(userDomain => userDomain.DomainId);
    }
}
