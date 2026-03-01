using Bogus;
using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Domain.Enums;
using Lar.TesteDotNet.Tests.Shared.Extensions;

namespace Lar.TesteDotNet.Tests.Shared.Factories;

public static class TelefoneFactory
{
    private static readonly Faker Faker = new("pt_BR");

    public static Telefone GetTelefone()
    {
        var telefone = CreateTelefone().Value;
        telefone.GenerateIds();
        return telefone;
    }

    public static DomainResult<Telefone> CreateTelefone(string propName, object? value)
    {
        var pessoaId = propName == nameof(Telefone.PessoaId)
            ? long.Parse(value?.ToString()!)
            : 1;

        var tipo = propName == nameof(Telefone.Tipo)
            ? (TipoTelefone)(int.Parse(value?.ToString()!))
            : TipoTelefone.Celular;

        var numero = propName == nameof(Telefone.Numero)
            ? value?.ToString()
            : RandomNumero();

        return Telefone.Create(pessoaId, tipo, numero);
    }

    private static DomainResult<Telefone> CreateTelefone()
    {
        return Telefone.Create(1, TipoTelefone.Celular, RandomNumero());
    }

    private static string RandomNumero()
    {
        return Faker.Phone.PhoneNumber("###########");
    }
}
