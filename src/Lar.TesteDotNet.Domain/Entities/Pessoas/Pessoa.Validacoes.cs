using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Errors;
using Lar.TesteDotNet.Domain.ValueObjects;

namespace Lar.TesteDotNet.Domain.Entities.Pessoas;

public partial class Pessoa
{
    private static List<Error> CanCreate(string? nome, DomainResult<Cpf> cpf, DateTime dataNascimento)
    {
        return new List<Error>()
            .AddErrors(ValidateNome(nome))
            .AddErrors(ValidateCpf(cpf))
            .AddErrors(ValidateDataNascimento(dataNascimento));
    }

    private static List<Error> CanUpdate(string? nome, DomainResult<Cpf> cpf, DateTime dataNascimento)
    {
        return new List<Error>()
            .AddErrors(ValidateNome(nome))
            .AddErrors(ValidateCpf(cpf))
            .AddErrors(ValidateDataNascimento(dataNascimento));
    }

    private List<Error> CanActivate()
    {
        return new List<Error>()
            .BeTrue(!Ativo, nameof(Ativo), nameof(Pessoa), "Pessoa já está ativa.", "invalid_operation");
    }

    private List<Error> CanDeactivate()
    {
        return new List<Error>()
            .BeTrue(Ativo, nameof(Ativo), nameof(Pessoa), "Pessoa já está inativa.", "invalid_operation");
    }

    private static List<Error> ValidateNome(string? nome)
    {
        return new List<Error>()
            .NotNull(nome, nameof(Nome), nameof(Pessoa))
            .JoinWhenEmpty(v => v.Length(nome, nameof(Nome), nameof(Pessoa), 2, 200));
    }

    private static List<Error> ValidateCpf(DomainResult<Cpf>? cpf)
    {
        return new List<Error>()
            .NotNull(cpf, nameof(Cpf), nameof(Pessoa))
            .AddErrors(cpf!.Errors.ToList());
    }

    private static List<Error> ValidateDataNascimento(DateTime dataNascimento)
    {
        return new List<Error>()
            .BeTrue(
                dataNascimento != default && dataNascimento.Date <= DateTime.UtcNow.Date,
                nameof(DataNascimento),
                nameof(Pessoa),
                "Data de nascimento inválida.",
                "invalid_value");
    }
}
