namespace Cofi.Identity.Mapping;

sealed class UserCreated_MP : Profile
{
    public UserCreated_MP()
    {
        CreateMap<User, UserCreated>();
    }
}