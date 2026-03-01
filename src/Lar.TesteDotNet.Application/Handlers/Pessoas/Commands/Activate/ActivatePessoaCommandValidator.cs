using Lar.TesteDotNet.Application.Validation;
using Lar.TesteDotNet.Domain.Entities.Pessoas;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Activate;

public sealed class ActivatePessoaCommandValidator : ValidatorBase<ActivatePessoaCommand>
{
    public ActivatePessoaCommandValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(x => x.Id)
            .Exists(UnitOfWork.GetRepository<Pessoa>())
            .WithMessage("Pessoa não cadastrada.");
    }
}
