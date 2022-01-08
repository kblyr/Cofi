namespace Cofi.Identity.Entities;

record User
{
    public int Id { get; init; }
    public string Username { get; set; } = default!;
    public string HashedPassword { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public bool IsActive { get; set; }
    public bool IsPasswordChangeRequired { get; set; }
    public bool IsEmailVerified { get; set; }
}