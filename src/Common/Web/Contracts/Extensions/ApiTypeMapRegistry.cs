using System.Reflection;

namespace Cofi.Contracts;

public static class ApiTypeMapRegistryExtensions
{
    static readonly Type _apiTypeMapRegistrationType = typeof(ApiTypeMapRegistration);

    public static ApiTypeMapRegistry RegisterApiTypeMaps(this ApiTypeMapRegistry registry, Assembly assemblyMarker)
    {
        var registrationTypes = assemblyMarker.GetTypes()
            .Where(registrationType =>
                registrationType.IsClass
                && !registrationType.IsAbstract
                && !registrationType.IsGenericType
                && registrationType.GetConstructor(Type.EmptyTypes) is not null
                && _apiTypeMapRegistrationType.IsAssignableFrom(registrationType)
            );

        if (registrationTypes is not null)
        {
            foreach (var registrationType in registrationTypes)
            {
                var instance = (ApiTypeMapRegistration?)Activator.CreateInstance(registrationType);
                instance?.Register(registry);
            }
        }

        return registry;
    }

    public static ApiTypeMapRegistry RegisterApiTypeMaps(this ApiTypeMapRegistry registry) => registry.RegisterApiTypeMaps(Assembly.GetCallingAssembly());
}
