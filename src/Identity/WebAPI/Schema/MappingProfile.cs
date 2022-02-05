using AutoMapper;

namespace Cofi.Identity.Schema;

sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Requests
        CreateMap<CreateUserRequest, CreateUser>();
        CreateMap<SignupUserRequest, SignupUser>();

        // Responses
        CreateMap<UsernameAlreadyExists, UsernameAlreadyExistsResponse>();
        
        CreateMap<CreateUser.Response, CreateUserResponse>();
        CreateMap<SignupUser.Response, SignupUserResponse>();
    }
}