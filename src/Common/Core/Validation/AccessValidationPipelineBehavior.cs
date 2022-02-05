using MediatR;

namespace Cofi.Validation;

sealed class AccessValidationPipelineBehavior<TRequest> : IPipelineBehavior<TRequest, CofiResponse>
    where TRequest : CofiRequest
{
    readonly IAccessValidator _validator;
    readonly IAccessValidationConfiguration<TRequest> _config;

    public AccessValidationPipelineBehavior(IAccessValidator validator, IAccessValidationConfiguration<TRequest> config)
    {
        _validator = validator;
        _config = config;
    }

    public async Task<CofiResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<CofiResponse> next)
    {
        var context = new AccessValidationContext();
        _config.Configure(context, request);

        var result = await _validator.Validate(AccessValidationMode.All, context.Rules, cancellationToken).ConfigureAwait(false);

        if (result.IsSucceeded == false)
        {
            return new AccessValidationFailed
            {
                FailedRules = result.FailedRules.Select(rule => new AccessValidationFailed.FailedRuleObj
                {
                    Name = rule.Name,
                    Data = rule.Data
                })
            };
        }

        return await next().ConfigureAwait(false);
    }
}
