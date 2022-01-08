using System.ComponentModel;
namespace Cofi.Identity.Data;

static class DbSetExtensions
{
    public static async Task<bool> UsernameExistsAsync(this DbSet<User> users, string username, CancellationToken cancellationToken = default) => await users
        .AsNoTracking()
        .Where(user => user.Username == username)
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);

    public static async Task<bool> EmailAddressExistsAsync(this DbSet<User> users, string emailAddress, CancellationToken cancellationToken = default) => await users
        .AsNoTracking()
        .Where(user => user.EmailAddress == emailAddress)
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);
}