namespace Cofi.Identity.MappingProfiles;

sealed class User_MP : Profile
{
    public User_MP()
    {
        CreateMap<SignupUser, User>();
    }
}