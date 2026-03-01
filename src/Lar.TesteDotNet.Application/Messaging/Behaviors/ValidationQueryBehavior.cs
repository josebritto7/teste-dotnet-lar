using FluentValidation;
using FluentValidation.Results;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;

namespace Lar.TesteDotNet.Application.Messaging.Behaviors;

public sealed class ValidationQueryBehavior<TQuery, TResponse>(
    IEnumerable<IValidator<TQuery>> validators,
    IQueryHandler<TQuery, TResponse> inner)
    : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TQuery>(query);
            var failures = new List<ValidationFailure>();

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);
                if (!result.IsValid) failures.AddRange(result.Errors);
            }

            if (failures.Count > 0) throw new ValidationException(failures);
        }

        return await inner.Handle(query, cancellationToken);
    }
}