using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Domain.Errors;
using Lar.TesteDotNet.Domain.Extensions;

namespace Lar.TesteDotNet.Domain.ValueObjects;

public sealed class Cpf : ValueObject
{
    private Cpf(string digitos)
    {
        Digitos = digitos;
    }

    private string Digitos { get; }

    public static DomainResult<Cpf> Create(string? input)
    {
        input = input?.OnlyDigits();
        var errors = Validate(input);

        return errors.Any()
            ? DomainResult<Cpf>.Fail(errors)
            : DomainResult<Cpf>.Ok(new Cpf(input!));
    }

    public override string ToString()
    {
        return Digitos;
    }

    private static bool HasValidDigits(string cpf)
    {
        if (cpf.Length != 11) return false;
        if (cpf.All(c => c == cpf[0])) return false;

        var soma1 = 0;
        for (var i = 0; i < 9; i++)
            soma1 += (cpf[i] - '0') * (10 - i);

        var resto1 = soma1 % 11;
        var dv1 = resto1 < 2 ? 0 : 11 - resto1;
        if (cpf[9] - '0' != dv1) return false;

        var soma2 = 0;
        for (var i = 0; i < 10; i++)
            soma2 += (cpf[i] - '0') * (11 - i);

        var resto2 = soma2 % 11;
        var dv2 = resto2 < 2 ? 0 : 11 - resto2;
        return cpf[10] - '0' == dv2;
    }

    private static List<Error> Validate(string? cpf)
    {
        return new List<Error>()
            .NotNull(cpf, nameof(Cpf), nameof(Pessoa))
            .Length(cpf, nameof(Cpf), nameof(Pessoa), 11, 11)
            .JoinWhenEmpty(v =>
                v.BeTrue(
                    HasValidDigits(cpf!),
                    nameof(Cpf),
                    nameof(Pessoa),
                    "Cpf informado inválido.",
                    "invalid_value"));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Digitos;
    }

    public static implicit operator string(Cpf cpf)
    {
        return cpf.Digitos;
    }
}
