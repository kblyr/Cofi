namespace Cofi.Contracts;

public class ApiTypeMapRegistry
{
    readonly Dictionary<Type, Type> _mappings = new();

    public ApiTypeMapRegistry Register(Type type, Type apiType)
    {
        if (!_mappings.ContainsKey(type))
            _mappings.Add(type, apiType);

        return this;
    }

    public ApiTypeMapRegistry Register<T, TApi>() => Register(typeof(T), typeof(TApi));

    public Type GetFor(Type type) => _mappings[type];

    public Type GetFor<T>() => GetFor(typeof(T));
}
