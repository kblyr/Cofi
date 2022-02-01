using AutoMapper;

namespace Cofi.Identity.Schema;

sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Requests
        CreateMap<SignupUserRequest, SignupUser>();

        // Responses
        CreateMap<SignupUser.Response, SignupUserResponse>();
        CreateMap<UsernameAlreadyExists, UsernameAlreadyExistsResponse>();
    }
}