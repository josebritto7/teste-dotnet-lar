using Bogus;
using Bogus.Extensions.Brazil;
using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Tests.Shared.Extensions;

namespace Lar.TesteDotNet.Tests.Shared.Factories;

public static class PessoaFactory
{
    private static readonly Faker Faker = new("pt_BR");

    public static Pessoa GetPessoa()
    {
        var pessoa = CreatePessoa().Value;
        pessoa.GenerateIds();
        return pessoa;
    }

    public static DomainResult<Pessoa> CreatePessoa(string propName, object? value)
    {
        var nome = propName == nameof(Pessoa.Nome)
            ? value?.ToString()
            : Faker.Name.FullName();

        var dataNascimento = propName == nameof(Pessoa.DataNascimento)
            ? DateTime.Parse(value?.ToString()!)
            : Faker.Date.Past(35, DateTime.UtcNow.AddYears(-18));

        var cpf = propName == nameof(Pessoa.Cpf)
            ? value?.ToString()
            : Faker.Person.Cpf(false);

        return Pessoa.Create(nome, cpf, dataNascimento);
    }

    private static DomainResult<Pessoa> CreatePessoa()
    {
        return Pessoa.Create(
            Faker.Name.FullName(),
            Faker.Person.Cpf(false),
            Faker.Date.Past(35, DateTime.UtcNow.AddYears(-18)));
    }
}
