using MediatR;

namespace Cofi.Validation;

sealed class ValidateAccessWrapperImpl<TRule> : ValidateAccessWrapper<TRule> where TRule : IAccessValidationRule
{
    public override async Task<bool> Validate(TRule rule, ServiceFactory serviceFactory, CancellationToken cancellationToken)
    {
        return await GetImpl<IValidateAccess<TRule>>(serviceFactory).Validate(rule, cancellationToken).ConfigureAwait(false);
    }

    public override async Task<bool> Validate(object rule, ServiceFactory serviceFactory, CancellationToken cancellationToken)
    {
        return await Validate((TRule)rule, serviceFactory, cancellationToken).ConfigureAwait(false);
    }
}