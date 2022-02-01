namespace Cofi.Identity.Contracts;

public record SignupUser : CofiRequest
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;

    public record Response : CofiResponse
    {
        public int Id { get; init; }

        public Response() { }

        public Response(int id)
        {
            Id = id;
        }
    }
}
