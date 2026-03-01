using Lar.TesteDotNet.Application.Validation;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Domain.Extensions;
using Lar.TesteDotNet.Domain.ValueObjects;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Update;

public sealed class UpdatePessoaCommandValidator : ValidatorBase<UpdatePessoaCommand>
{
    public UpdatePessoaCommandValidator(IUnitOfWork uow) : base(uow)
    {
        RuleFor(x => x.Id)
            .Exists(UnitOfWork.GetRepository<Pessoa>())
            .WithMessage("Pessoa não cadastrada.");

        RuleFor(c => c)
            .Must(UniqueCpf)
            .OverridePropertyName(p => p.Cpf)
            .WithMessage("Cpf já cadastrado.");
    }

    private bool UniqueCpf(UpdatePessoaCommand command)
    {
        var cpfRes = Cpf.Create(command.Cpf);
        if (!cpfRes.IsSuccess) return true;

        return !UnitOfWork.GetRepository<Pessoa>()
            .Exists(c => c.Cpf.Equals(cpfRes.Value) && c.Id != command.Id);
    }
}
