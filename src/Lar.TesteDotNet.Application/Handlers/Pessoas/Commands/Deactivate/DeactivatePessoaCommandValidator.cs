using Lar.TesteDotNet.Application.Validation;
using Lar.TesteDotNet.Domain.Entities.Pessoas;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Deactivate;

public sealed class DeactivatePessoaCommandValidator : ValidatorBase<DeactivatePessoaCommand>
{
    public DeactivatePessoaCommandValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(x => x.Id)
            .Exists(UnitOfWork.GetRepository<Pessoa>())
            .WithMessage("Pessoa não cadastrada.");
    }
}
