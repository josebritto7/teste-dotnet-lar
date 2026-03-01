using AwesomeAssertions;
using Bogus.Extensions.Brazil;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Domain.Extensions;
using Lar.TesteDotNet.Tests.Shared.Common;
using Lar.TesteDotNet.Tests.Shared.Factories;

namespace Lar.TesteDotNet.Tests.Domain.Entities.Pessoas;

public class PessoaTests : Test
{
    [TestCase(true)]
    [TestCase(false)]
    public void Create_ShouldSucceed_WithValidData(bool formatCpf)
    {
        var nome = Faker.Name.FullName();
        var cpf = Faker.Person.Cpf(formatCpf);
        var dataNascimento = Faker.Date.Past(35, DateTime.UtcNow.AddYears(-18));

        var result = Pessoa.Create(nome, cpf, dataNascimento);

        var pessoa = result.Value;

        result.IsSuccess.Should().BeTrue();
        pessoa.Nome.Should().Be(nome);
        pessoa.Cpf.ToString().Should().Be(cpf.OnlyDigits());
        pessoa.DataNascimento.Should().Be(dataNascimento.Date);
        pessoa.Ativo.Should().BeTrue();
        pessoa.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 2));
    }

    [Test]
    public void Deactivate_ShouldSucceed_WhenActive()
    {
        var pessoa = PessoaFactory.GetPessoa();

        var result = pessoa.Deactivate();

        result.IsSuccess.Should().BeTrue();
        pessoa.Ativo.Should().BeFalse();
        pessoa.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 2));
    }

    [Test]
    public void Activate_ShouldSucceed_WhenInactive()
    {
        var pessoa = PessoaFactory.GetPessoa();

        pessoa.Deactivate();
        var result = pessoa.Activate();

        result.IsSuccess.Should().BeTrue();
        pessoa.Ativo.Should().BeTrue();
        pessoa.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 2));
    }

    [Test]
    public void UpdateDetails_ShouldSucceed_WithValidData()
    {
        var pessoa = PessoaFactory.GetPessoa();

        var nome = Faker.Name.FullName();
        var cpf = Faker.Person.Cpf(false);
        var dataNascimento = Faker.Date.Past(30, DateTime.UtcNow.AddYears(-18));
        var createdAtBefore = pessoa.CreatedAt;

        var result = pessoa.UpdateDetails(nome, cpf, dataNascimento);

        result.IsSuccess.Should().BeTrue();
        pessoa.Nome.Should().Be(nome);
        pessoa.Cpf.ToString().Should().Be(cpf.OnlyDigits());
        pessoa.DataNascimento.Should().Be(dataNascimento.Date);
        pessoa.CreatedAt.Should().Be(createdAtBefore);
        pessoa.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 0, 2));
    }
}
