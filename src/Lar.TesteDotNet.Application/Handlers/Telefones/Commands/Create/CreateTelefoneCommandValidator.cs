using Lar.TesteDotNet.Application.Validation;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Domain.Extensions;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Create;

public sealed class CreateTelefoneCommandValidator : ValidatorBase<CreateTelefoneCommand>
{
    public CreateTelefoneCommandValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(x => x.PessoaId)
            .Exists(UnitOfWork.GetRepository<Pessoa>())
            .WithMessage("Pessoa não cadastrada.");

        RuleFor(c => c)
            .Must(UniqueNumero)
            .OverridePropertyName(p => p.Numero)
            .WithMessage("Telefone já cadastrado para esta pessoa.");
    }

    private bool UniqueNumero(CreateTelefoneCommand command)
    {
        var numeroNormalizado = command.Numero.OnlyDigits();

        return !UnitOfWork.GetRepository<Telefone>()
            .Exists(c => c.PessoaId == command.PessoaId && c.Numero.Equals(numeroNormalizado));
    }
}
