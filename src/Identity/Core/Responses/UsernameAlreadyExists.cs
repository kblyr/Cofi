namespace Cofi.Identity.Responses;

public record UsernameAlreadyExists(string Username) : FailedResponse;