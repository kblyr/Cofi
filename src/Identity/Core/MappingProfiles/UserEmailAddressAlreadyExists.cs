namespace Cofi.Identity.MappingProfiles;

sealed class UserEmailAddressAlreadyExists_MP : Profile
{
    public UserEmailAddressAlreadyExists_MP()
    {
        CreateMap<CreateUser, UserEmailAddressAlreadyExists>();
    }
}
