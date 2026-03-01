using Lar.TesteDotNet.Application.Validation;
using Lar.TesteDotNet.Domain.Entities.Telefones;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Delete;

public sealed class DeleteTelefoneCommandValidator : ValidatorBase<DeleteTelefoneCommand>
{
    public DeleteTelefoneCommandValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(x => x.Id)
            .Must(TelefoneExistsForPessoa)
            .WithMessage("Telefone não cadastrado para esta pessoa.");
    }

    private bool TelefoneExistsForPessoa(DeleteTelefoneCommand command, long id)
    {
        return UnitOfWork.GetRepository<Telefone>()
            .Exists(c => c.Id == id && c.PessoaId == command.PessoaId);
    }
}
