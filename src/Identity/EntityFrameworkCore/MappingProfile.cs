using AutoMapper;

namespace Cofi.Identity;

sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUser, User>();
        CreateMap<SignupUser, User>();
        
        CreateMap<User, UserCreated>();
    }
}