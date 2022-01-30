namespace Cofi.Identity.Utilities;

public interface IUserPasswordHash
{
    public string ComputeHash(string password);
}
