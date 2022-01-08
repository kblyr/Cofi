namespace Cofi.Identity.MappingProfiles;

sealed class SendUserEmailVerification_MP : Profile
{
    public SendUserEmailVerification_MP()
    {
        CreateMap<UserCreated, SendUserEmailVerification>();
    }
}