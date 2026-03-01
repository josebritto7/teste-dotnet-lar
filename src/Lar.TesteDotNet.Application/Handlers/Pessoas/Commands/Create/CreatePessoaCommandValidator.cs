using Lar.TesteDotNet.Application.Validation;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Domain.Extensions;
using Lar.TesteDotNet.Domain.ValueObjects;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Create;

public sealed class CreatePessoaCommandValidator : ValidatorBase<CreatePessoaCommand>
{
    public CreatePessoaCommandValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(c => c.Cpf.OnlyDigits())
            .Must(UniqueCpf)
            .WithMessage("Cpf já cadastrado.");
    }

    private bool UniqueCpf(string cpf)
    {
        var cpfRes = Cpf.Create(cpf);
        if (!cpfRes.IsSuccess) return true;

        return !UnitOfWork.GetRepository<Pessoa>().Exists(c => c.Cpf.Equals(cpfRes.Value));
    }
}
