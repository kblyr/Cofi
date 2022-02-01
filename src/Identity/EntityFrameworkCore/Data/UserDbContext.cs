using Microsoft.EntityFrameworkCore;

namespace Cofi.Identity.Data;

sealed class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}