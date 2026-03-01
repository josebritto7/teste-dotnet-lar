using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Domain.Interfaces;
using Lar.TesteDotNet.Domain.ValueObjects;

namespace Lar.TesteDotNet.Domain.Entities.Pessoas;

public partial class Pessoa : Entity, IAggregateRoot
{
    private Pessoa()
    {
    }

    private Pessoa(
        string nome,
        Cpf cpf,
        DateTime dataNascimento)
    {
        Nome = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento.Date;
        Ativo = true;
        CreatedAt = DateTime.UtcNow;
    }

    public string Nome { get; private set; } = string.Empty;
    public Cpf Cpf { get; private set; } = null!;
    public DateTime DataNascimento { get; private set; }
    public bool Ativo { get; private set; }
    public List<Telefone> Telefones { get; private set; } = [];

    public static DomainResult<Pessoa> Create(
        string? nome,
        string? cpf,
        DateTime dataNascimento)
    {
        var cpfRes = Cpf.Create(cpf);
        var errors = CanCreate(nome, cpfRes, dataNascimento);

        return DomainResult<Pessoa>.From(
            () => new Pessoa(nome!, cpfRes.Value, dataNascimento),
            errors);
    }
}
