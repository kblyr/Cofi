using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cofi.Identity.EntityTypeConfigurations;

sealed class UserRole_ETC : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRole", DatabaseDefaults.Schema);

        builder.HasOne(userRole => userRole.User)
            .WithMany()
            .HasForeignKey(userRole => userRole.UserId);

        builder.HasOne(userRole => userRole.Role)
            .WithMany()
            .HasForeignKey(userRole => userRole.RoleId);
    }
}
