using AwesomeAssertions;
using Bogus.Extensions.Brazil;
using Lar.TesteDotNet.Domain.Extensions;
using Lar.TesteDotNet.Domain.ValueObjects;
using Lar.TesteDotNet.Tests.Shared.Common;

namespace Lar.TesteDotNet.Tests.Domain.ValueObjects;

public class CpfTests : Test
{
    [TestCase(true)]
    [TestCase(false)]
    public void Create_ValidCpf_ReturnNormalized(bool includeSymbols)
    {
        var tmpCpf = Faker.Person.Cpf(includeSymbols);

        var cpf = Cpf.Create(tmpCpf);

        cpf.IsSuccess.Should().BeTrue();
        cpf.Value.ToString().Should().Be(tmpCpf.OnlyDigits());
    }

    [TestCase("")]
    [TestCase("123")]
    public void MustFailWithInvalidLengthCpf(string value)
    {
        var cpf = Cpf.Create(value);

        cpf.IsSuccess.Should().BeFalse();
        cpf.Errors.Should().HaveCount(1);
        cpf.Errors[0].Message.Should()
            .Be($"Propriedade Cpf com tamanho inválido, esperado 11, tamanho atual {value.Length}.");
    }

    [TestCase("00000000000")]
    [TestCase("12345678900")]
    [TestCase("99999999999")]
    public void MustFailWithInvalidCpf(string value)
    {
        var cpf = Cpf.Create(value);

        cpf.IsSuccess.Should().BeFalse();
        cpf.Errors.Should().HaveCount(1);
        cpf.Errors[0].Message.Should().Be("Cpf informado inválido.");
    }
}
