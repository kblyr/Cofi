using System.Text;
using System.Security.Cryptography;

namespace Cofi.Identity.Utilities;

sealed class UserPasswordHash : IUserPasswordHash
{
    public string ComputeHash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return "";

        using var algo = SHA512.Create();
        var passwordData = Encoding.UTF8.GetBytes(password);
        var cipherData = algo.ComputeHash(passwordData);
        return new StringBuilder(BitConverter.ToString(cipherData))
            .Replace("-", "")
            .ToString();
    }
}