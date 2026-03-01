using FluentValidation;
using FluentValidation.Results;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;

namespace Lar.TesteDotNet.Application.Messaging.Behaviors;

public sealed class ValidationCommandBehavior<TCommand>(
    IEnumerable<IValidator<TCommand>> validators,
    ICommandHandler<TCommand> inner)
    : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public async Task Handle(TCommand command, CancellationToken cancellationToken)
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

            if (failures.Count > 0) throw new ValidationException(failures);
        }

        await inner.Handle(command, cancellationToken);
    }
}