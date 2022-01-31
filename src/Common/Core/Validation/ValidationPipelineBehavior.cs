using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Cofi.Validation;

sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, CofiResponse>
    where TRequest : CofiRequest
{
    readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<CofiResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<CofiResponse> next)
    {
        var failures = new List<ValidationFailure>();
        
        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

            if (result.Errors is { Count: > 0 })
                failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
        {
            return new ValidationFailed
            {
                Failures = failures.Select(failure => new ValidationFailed.FailureObj
                {
                    PropertyName = failure.PropertyName,
                    ErrorMessage = failure.ErrorMessage
                })
            };
        }

        return await next().ConfigureAwait(false);
    }
}