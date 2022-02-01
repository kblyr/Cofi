using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cofi.Identity.EntityTypeConfigurations;

sealed class User_ETC : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", DatabaseDefaults.Schema);

        builder.Property(user => user.Status)
            .HasConversion<byte>();
    }
}