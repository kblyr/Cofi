namespace Cofi.Identity.Entities;

record User
{
    public int Id { get; init; }
    public string Username { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string HashedPassword { get; set; } = "";
    public bool IsActive { get; set; }
    public bool IsEmailAddressVerified { get; set; }
    public bool IsPasswordChangeRequired { get; set; }
}