using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Domain.Enums;
using Lar.TesteDotNet.Domain.Extensions;
using Lar.TesteDotNet.Domain.Interfaces;

namespace Lar.TesteDotNet.Domain.Entities.Telefones;

public partial class Telefone : Entity, IAggregateRoot
{
    private Telefone()
    {
    }

    private Telefone(long pessoaId, TipoTelefone tipo, string numero)
    {
        PessoaId = pessoaId;
        Tipo = tipo;
        Numero = numero;
        CreatedAt = DateTime.UtcNow;
    }

    public long PessoaId { get; private set; }
    public Pessoa Pessoa { get; private set; } = null!;
    public TipoTelefone Tipo { get; private set; }
    public string Numero { get; private set; } = string.Empty;

    public static DomainResult<Telefone> Create(long pessoaId, TipoTelefone tipo, string? numero)
    {
        var numeroNormalizado = numero?.OnlyDigits();
        var errors = CanCreate(pessoaId, tipo, numeroNormalizado);

        return DomainResult<Telefone>.From(
            () => new Telefone(pessoaId, tipo, numeroNormalizado!),
            errors);
    }
}
