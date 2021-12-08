namespace Cofi.Identity.Requests;

public record SendUserEmailVerification
{
    public int UserId { get; init; }
    public string EmailAddress { get; init; } = "";

    public SendUserEmailVerification() { }

    public SendUserEmailVerification(int userId, string emailAddress)
    {
        UserId = userId;
        EmailAddress = emailAddress;
    }
}
