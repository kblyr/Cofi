using MediatR;

namespace Cofi.Validation;

abstract class ValidateAccessWrapperBase 
{
    public abstract Task<bool> Validate(object rule, ServiceFactory serviceFactory, CancellationToken cancellationToken);

    protected static T GetImpl<T>(ServiceFactory serviceFactory)
    {
        return serviceFactory.GetInstance<T>() ?? throw new CofiException($"Cannot find implementation for type '{typeof(T).FullName}'");
    }
}
