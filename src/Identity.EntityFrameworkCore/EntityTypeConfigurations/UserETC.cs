namespace Cofi.Identity.EntityTypeConfigurations;

sealed class UserETC : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(Tables.Identity.User, Schemas.Identity);
    }
}