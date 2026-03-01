using AwesomeAssertions;
using Bogus.Extensions.Brazil;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Tests.Shared.Common;
using Lar.TesteDotNet.Tests.Shared.Factories;

namespace Lar.TesteDotNet.Tests.Domain.Entities.Pessoas;

public class PessoaValidacoesTests : Test
{
    [TestCase("")]
    [TestCase("a")]
    [TestCase(null)]
    public void Create_ShouldFail_WithInvalidNome(string? nome)
    {
        var result = PessoaFactory.CreatePessoa(nameof(Pessoa.Nome), nome);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);

        if (nome is null)
            result.Errors.First().Message.Should()
                .Contain($"É obrigatório informar a propriedade {nameof(Pessoa.Nome)}.");
        else
            result.Errors.First().Message.Should().Contain($"Propriedade {nameof(Pessoa.Nome)} com tamanho inválido");
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("123")]
    public void Create_ShouldFail_WithInvalidCpf(string? cpf)
    {
        var result = PessoaFactory.CreatePessoa(nameof(Pessoa.Cpf), cpf);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [TestCase("0001-01-01")]
    [TestCase("3000-01-01")]
    public void Create_ShouldFail_WithInvalidDataNascimento(string dataNascimento)
    {
        var result = PessoaFactory.CreatePessoa(nameof(Pessoa.DataNascimento), dataNascimento);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Be("Data de nascimento inválida.");
    }

    [Test]
    public void Create_ShouldAggregateErrors_WhenMultipleInvalidInputs()
    {
        string? nome = null;
        var cpf = "";
        var dataNascimento = default(DateTime);

        var result = Pessoa.Create(nome, cpf, dataNascimento);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Count.Should().BeGreaterThan(2);
    }

    [Test]
    public void Activate_ShouldFail_WhenAlreadyActive()
    {
        var pessoa = PessoaFactory.GetPessoa();

        var result = pessoa.Activate();

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Contain("Pessoa já está ativa.");
        pessoa.Ativo.Should().BeTrue();
    }

    [Test]
    public void Deactivate_ShouldFail_WhenAlreadyInactive()
    {
        var pessoa = PessoaFactory.GetPessoa();
        pessoa.Deactivate();

        var result = pessoa.Deactivate();

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Contain("Pessoa já está inativa.");
        pessoa.Ativo.Should().BeFalse();
    }

    [TestCase("")]
    [TestCase("a")]
    [TestCase(null)]
    public void UpdateDetails_ShouldFail_WithInvalidNome(string? nome)
    {
        var pessoa = PessoaFactory.GetPessoa();
        var cpf = Faker.Person.Cpf(false);
        var dataNascimento = Faker.Date.Past(20, DateTime.UtcNow.AddYears(-18));

        var result = pessoa.UpdateDetails(nome!, cpf, dataNascimento);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("123")]
    public void UpdateDetails_ShouldFail_WithInvalidCpf(string? cpf)
    {
        var pessoa = PessoaFactory.GetPessoa();
        var nome = Faker.Name.FullName();
        var dataNascimento = Faker.Date.Past(20, DateTime.UtcNow.AddYears(-18));

        var result = pessoa.UpdateDetails(nome, cpf!, dataNascimento);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [TestCase("0001-01-01")]
    [TestCase("3000-01-01")]
    public void UpdateDetails_ShouldFail_WithInvalidDataNascimento(string dataNascimento)
    {
        var pessoa = PessoaFactory.GetPessoa();
        var nome = Faker.Name.FullName();
        var cpf = Faker.Person.Cpf(false);

        var result = pessoa.UpdateDetails(nome, cpf, DateTime.Parse(dataNascimento));

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Message.Should().Be("Data de nascimento inválida.");
    }
}
