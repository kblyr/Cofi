namespace Cofi.Identity.MappingProfiles;

sealed class User_MP : Profile
{
    public User_MP()
    {
        CreateMap<CreateUser, User>()
            .ForMember(dest => dest.Id, config => config.Ignore())
            .ForMember(dest => dest.HashedPassword, config => config.Ignore());
    }
}