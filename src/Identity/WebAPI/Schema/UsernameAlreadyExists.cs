using AutoMapper;

namespace Cofi.Identity.Schema;

public record UsernameAlreadyExistsResponse
{
    public string Username { get; init; } = default!;
}

sealed class UsernameAlreadyExistsResponse_Mapping : Profile
{
    public UsernameAlreadyExistsResponse_Mapping()
    {
        CreateMap<UsernameAlreadyExists, UsernameAlreadyExistsResponse>();
    }
}