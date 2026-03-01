using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Enums;
using Lar.TesteDotNet.Domain.Errors;

namespace Lar.TesteDotNet.Domain.Entities.Telefones;

public partial class Telefone
{
    private static List<Error> CanCreate(long pessoaId, TipoTelefone tipo, string? numero)
    {
        return new List<Error>()
            .AddErrors(ValidatePessoa(pessoaId))
            .AddErrors(ValidateTipo(tipo))
            .AddErrors(ValidateNumero(numero));
    }

    private static List<Error> CanUpdate(TipoTelefone tipo, string? numero)
    {
        return new List<Error>()
            .AddErrors(ValidateTipo(tipo))
            .AddErrors(ValidateNumero(numero));
    }

    private static List<Error> ValidatePessoa(long pessoaId)
    {
        return new List<Error>()
            .Id(pessoaId, nameof(PessoaId), nameof(Telefone), "Pessoa inválida.");
    }

    private static List<Error> ValidateTipo(TipoTelefone tipo)
    {
        return new List<Error>()
            .BeTrue(
                Enum.IsDefined(tipo),
                nameof(Tipo),
                nameof(Telefone),
                "Tipo de telefone inválido.",
                "invalid_value");
    }

    private static List<Error> ValidateNumero(string? numero)
    {
        return new List<Error>()
            .NotNull(numero, nameof(Numero), nameof(Telefone))
            .JoinWhenEmpty(v => v.Length(numero, nameof(Numero), nameof(Telefone), 10, 11));
    }
}
