namespace Cofi.Identity.Contracts;

public record SignupUser : Request
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;

    public record Response : Cofi.Contracts.Response
    {
        public int Id { get; init; }

        public Response() { }

        public Response(int id)
        {
            Id = id;
        }
    }
}
