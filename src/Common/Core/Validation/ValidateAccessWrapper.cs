using MediatR;

namespace Cofi.Validation;

abstract class ValidateAccessWrapper<TRule> : ValidateAccessWrapperBase where TRule : IAccessValidationRule
{
    public abstract Task<bool> Validate(TRule rule, ServiceFactory serviceFactory, CancellationToken cancellationToken);
}
