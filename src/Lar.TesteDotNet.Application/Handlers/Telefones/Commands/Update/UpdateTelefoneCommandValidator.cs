using Lar.TesteDotNet.Application.Validation;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Domain.Extensions;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Update;

public sealed class UpdateTelefoneCommandValidator : ValidatorBase<UpdateTelefoneCommand>
{
    public UpdateTelefoneCommandValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(x => x.Id)
            .Must(TelefoneExistsForPessoa)
            .WithMessage("Telefone não cadastrado para esta pessoa.");

        RuleFor(c => c)
            .Must(UniqueNumero)
            .OverridePropertyName(p => p.Numero)
            .WithMessage("Telefone já cadastrado para esta pessoa.");
    }

    private bool TelefoneExistsForPessoa(UpdateTelefoneCommand command, long id)
    {
        return UnitOfWork.GetRepository<Telefone>()
            .Exists(c => c.Id == id && c.PessoaId == command.PessoaId);
    }

    private bool UniqueNumero(UpdateTelefoneCommand command)
    {
        var numeroNormalizado = command.Numero.OnlyDigits();

        return !UnitOfWork.GetRepository<Telefone>()
            .Exists(c => c.PessoaId == command.PessoaId &&
                         c.Numero.Equals(numeroNormalizado) &&
                         c.Id != command.Id);
    }
}
