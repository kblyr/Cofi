namespace Cofi.Identity.Utilities;

public interface IUserPasswordCryptoHash
{
    public string ComputeHash(string password);
}