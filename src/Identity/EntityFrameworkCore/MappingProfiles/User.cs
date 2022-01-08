namespace Cofi.Identity.MappingProfiles;

sealed class User_MP : Profile
{
    public User_MP()
    {
        CreateMap<CreateUser, User>()
            .ForMember(dest => dest.Id, config => config.Ignore())
            .ForMember(dest => dest.HashedPassword, config => config.Ignore())
            .ForMember(dest => dest.IsDeleted, config => config.Ignore())
            .ForMember(dest => dest.InsertedById, config => config.Ignore())
            .ForMember(dest => dest.InsertedOn, config => config.Ignore())
            .ForMember(dest => dest.UpdatedById, config => config.Ignore())
            .ForMember(dest => dest.UpdatedOn, config => config.Ignore())
            .ForMember(dest => dest.DeletedById, config => config.Ignore())
            .ForMember(dest => dest.DeletedOn, config => config.Ignore());
    }
}