namespace Cofi.Identity;

sealed class ApiTypeMappingRegistration : IApiTypeMapRegistration
{
    public void Register(ApiTypeMapRegistry registry)
    {
        registry
            .Register<SignupUser.Response, SignupUserResponse>()
            .Register<UsernameAlreadyExists, UsernameAlreadyExistsResponse>();
    }
}
