namespace Cofi.Identity.Data;

sealed class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Cofi.Identity.EntityFrameworkCore.AssemblyMarker.Assembly);
    }
}