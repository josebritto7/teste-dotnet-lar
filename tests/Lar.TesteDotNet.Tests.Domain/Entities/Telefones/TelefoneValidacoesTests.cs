using AwesomeAssertions;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Domain.Enums;
using Lar.TesteDotNet.Tests.Shared.Common;
using Lar.TesteDotNet.Tests.Shared.Factories;

namespace Lar.TesteDotNet.Tests.Domain.Entities.Telefones;

public class TelefoneValidacoesTests : Test
{
    [TestCase("")]
    [TestCase("123")]
    [TestCase(null)]
    public void Create_ShouldFail_WithInvalidNumero(string? numero)
    {
        var result = TelefoneFactory.CreateTelefone(nameof(Telefone.Numero), numero);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void Create_ShouldFail_WithInvalidPessoaId(long pessoaId)
    {
        var result = TelefoneFactory.CreateTelefone(nameof(Telefone.PessoaId), pessoaId);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Be("Pessoa inválida.");
    }

    [TestCase(0)]
    [TestCase(99)]
    public void Create_ShouldFail_WithInvalidTipo(int tipo)
    {
        var result = TelefoneFactory.CreateTelefone(nameof(Telefone.Tipo), tipo);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Be("Tipo de telefone inválido.");
    }

    [Test]
    public void Create_ShouldAggregateErrors_WhenMultipleInvalidInputs()
    {
        long pessoaId = 0;
        const string numero = "";
        var tipo = (TipoTelefone)999;

        var result = Telefone.Create(pessoaId, tipo, numero);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Count.Should().BeGreaterThan(2);
    }

    [TestCase("")]
    [TestCase("123")]
    [TestCase(null)]
    public void UpdateDetails_ShouldFail_WithInvalidNumero(string? numero)
    {
        var telefone = TelefoneFactory.GetTelefone();

        var result = telefone.UpdateDetails(TipoTelefone.Celular, numero);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [TestCase(0)]
    [TestCase(99)]
    public void UpdateDetails_ShouldFail_WithInvalidTipo(int tipo)
    {
        var telefone = TelefoneFactory.GetTelefone();

        var result = telefone.UpdateDetails((TipoTelefone)tipo, Faker.Phone.PhoneNumber("##########"));

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Be("Tipo de telefone inválido.");
    }
}
