using FluentValidation.Results;

namespace Cofi.Validation;

sealed class ValidationBehavior<TRequest> : IPipelineBehavior<TRequest, CofiResponse> where TRequest : CofiRequest
{
    readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<CofiResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<CofiResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(context, cancellationToken).ConfigureAwait(false);

            if (result.Errors is not null)
                failures.AddRange(result.Errors);
        }

        if (failures.Any())
        {
            return new ValidationFailedResponse
            {
                Failures = failures.Select(failure => new ValidationFailedResponse.ValidationFailureObj 
                { 
                    PropertyName = failure.PropertyName, 
                    ErrorMessage = failure.ErrorMessage 
                })
            };
        }

        return await next().ConfigureAwait(false);
    }
}
