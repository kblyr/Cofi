namespace Cofi.Contracts;

public interface IApiTypeMapRegistration
{
    void Register(ApiTypeMapRegistry registry);
}

sealed class ApiTypeMapRegistration : IApiTypeMapRegistration
{
    public void Register(ApiTypeMapRegistry registry)
    {
        registry.Register<ValidationFailed, ValidationFailedResponse>();
    }
}
