using AutoMapper;

namespace Cofi.Identity;

sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SignupUser, User>();
        
        CreateMap<User, UserSignedUp>();
    }
}