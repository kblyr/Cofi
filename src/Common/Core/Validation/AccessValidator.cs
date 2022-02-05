using System.Collections.Concurrent;
using MediatR;

namespace Cofi.Validation;

sealed class AccessValidator : AccessValidatorBase, IAccessValidator
{
    static readonly ConcurrentDictionary<Type, ValidateAccessWrapperBase> _validateAccessImpls = new();

    readonly ServiceFactory _serviceFactory;

    public AccessValidator(ServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    protected override async Task<bool> Validate(IAccessValidationRule rule, CancellationToken cancellationToken)
    {
        var ruleType = rule.GetType();

        var validateAccessImpl = _validateAccessImpls.GetOrAdd(ruleType,
            type => (ValidateAccessWrapperBase?)(Activator.CreateInstance(typeof(ValidateAccessWrapperImpl<>).MakeGenericType(ruleType)))
            ?? throw new CofiException($"Could not create wrapper for type '{ruleType.FullName}'")
        );

        return await validateAccessImpl.Validate(rule, _serviceFactory, cancellationToken).ConfigureAwait(false);
    }
}
