namespace Cofi.Identity.Events;

public record UserCreated
{
    public int Id { get; init; }
    public string Username { get; init; } = "";
    public string EmailAddress { get; init; } = "";
    public bool IsActive { get; init; }
    public bool IsEmailAddressVerified { get; init; }
    public bool IsPasswordChangeRequired { get; init; }
}
