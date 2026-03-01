using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Enums;
using Lar.TesteDotNet.Domain.Extensions;

namespace Lar.TesteDotNet.Domain.Entities.Telefones;

public partial class Telefone
{
    public DomainResult UpdateDetails(TipoTelefone tipo, string? numero)
    {
        var numeroNormalizado = numero?.OnlyDigits();
        var errors = CanUpdate(tipo, numeroNormalizado);

        return DomainResult.From(() => _UpdateDetails(tipo, numeroNormalizado!), errors);
    }

    private void _UpdateDetails(TipoTelefone tipo, string numero)
    {
        Tipo = tipo;
        Numero = numero;
        UpdatedAt = DateTime.UtcNow;
    }
}
