using Microsoft.EntityFrameworkCore;

namespace Cofi.Identity.Data;

sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserDomain> UserDomains => Set<UserDomain>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
}