namespace Cofi.Identity.MappingProfiles;

sealed class UsernameAlreadyExists_MP : Profile
{
    public UsernameAlreadyExists_MP()
    {
        CreateMap<CreateUser, UsernameAlreadyExists>();
    }
}