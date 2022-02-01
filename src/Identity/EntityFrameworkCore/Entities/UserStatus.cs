namespace Cofi.Identity.Entities;

public enum UserStatus : byte
{
    Pending = 1,
    Active = 2,
    Locked = 3,
    Deactivated = 4
}