using AutoMapper;

namespace Cofi.Contracts;

public class ApiTypeMapper
{
    readonly IMapper _mapper;
    readonly ApiTypeMapRegistry _registry;

    public ApiTypeMapper(IMapper mapper, ApiTypeMapRegistry registry)
    {
        _mapper = mapper;
        _registry = registry;
    }

    public object Map(object response, Type responseType) => _mapper.Map(response, responseType, _registry.GetFor(responseType));

    public object Map<T>(T response) where T : notnull => Map(response, typeof(T));
}
