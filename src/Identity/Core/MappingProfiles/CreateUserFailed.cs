namespace Cofi.Identity.MappingProfiles;

sealed class CreateUserFailed_MP : Profile
{
    public CreateUserFailed_MP()
    {
        CreateMap<UserEmailAddressAlreadyExists, CreateUserFailed>()
            .ForMember(dest => dest.EmailAddressAlreadyExists, config => config.MapFrom(src => src));

        CreateMap<UsernameAlreadyExists, CreateUserFailed>()
            .ForMember(dest => dest.UsernameAlreadyExists, config => config.MapFrom(src => src));
    }
}
