using FluentValidation;
using FluentValidation.Results;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Lar.TesteDotNet.Application.Messaging.Behaviors;

public sealed class ValidationCommandBehavior<TCommand, TResponse>(
    IEnumerable<IValidator<TCommand>> validators,
    ICommandHandler<TCommand, TResponse> inner)
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TCommand>(command);
            var failures = new List<ValidationFailure>();

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);
                if (!result.IsValid) failures.AddRange(result.Errors);
            }

            if (failures.Count > 0) throw new ValidationException(failures.ToString());
        }

        return await inner.Handle(command, cancellationToken);
    }
}