using AwesomeAssertions;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Domain.Enums;
using Lar.TesteDotNet.Domain.Extensions;
using Lar.TesteDotNet.Tests.Shared.Common;
using Lar.TesteDotNet.Tests.Shared.Factories;

namespace Lar.TesteDotNet.Tests.Domain.Entities.Telefones;

public class TelefoneTests : Test
{
    [TestCase(TipoTelefone.Celular)]
    [TestCase(TipoTelefone.Residencial)]
    [TestCase(TipoTelefone.Comercial)]
    public void Create_ShouldSucceed_WithValidData(TipoTelefone tipo)
    {
        var pessoaId = Faker.Random.Long(1, 1000);
        var numero = Faker.Phone.PhoneNumber("(##) #####-####");

        var result = Telefone.Create(pessoaId, tipo, numero);

        var telefone = result.Value;

        result.IsSuccess.Should().BeTrue();
        telefone.PessoaId.Should().Be(pessoaId);
        telefone.Tipo.Should().Be(tipo);
        telefone.Numero.Should().Be(numero.OnlyDigits());
        telefone.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 2));
    }

    [TestCase(TipoTelefone.Celular)]
    [TestCase(TipoTelefone.Residencial)]
    [TestCase(TipoTelefone.Comercial)]
    public void UpdateDetails_ShouldSucceed_WithValidData(TipoTelefone tipo)
    {
        var telefone = TelefoneFactory.GetTelefone();
        var originalCreatedAt = telefone.CreatedAt;
        var numero = Faker.Phone.PhoneNumber("(##) #####-####");

        var result = telefone.UpdateDetails(tipo, numero);

        result.IsSuccess.Should().BeTrue();
        telefone.Tipo.Should().Be(tipo);
        telefone.Numero.Should().Be(numero.OnlyDigits());
        telefone.CreatedAt.Should().Be(originalCreatedAt);
        telefone.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 2));
    }
}
