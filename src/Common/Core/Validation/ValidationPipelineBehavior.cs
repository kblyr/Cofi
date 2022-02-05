using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Cofi.Validation;

sealed class ValidationPipelineBehavior<TRequest> : IPipelineBehavior<TRequest, Response>
    where TRequest : Request
{
    readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<Response> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Response> next)
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
