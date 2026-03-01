using Lar.TesteDotNet.Domain.Common;
using Lar.TesteDotNet.Domain.ValueObjects;

namespace Lar.TesteDotNet.Domain.Entities.Pessoas;

public partial class Pessoa
{
    public DomainResult Activate()
    {
        var errors = CanActivate();
        return DomainResult.From(_Activate, errors);
    }

    public DomainResult Deactivate()
    {
        var errors = CanDeactivate();
        return DomainResult.From(_Deactivate, errors);
    }

    public DomainResult UpdateDetails(string nome, string cpf, DateTime dataNascimento)
    {
        var tmpCpf = Cpf.Create(cpf);
        var errors = CanUpdate(nome, tmpCpf, dataNascimento);

        return DomainResult.From(() => _UpdateDetails(nome, tmpCpf.Value, dataNascimento), errors);
    }

    private void _Activate()
    {
        Ativo = true;
        UpdatedAt = DateTime.UtcNow;
    }

    private void _Deactivate()
    {
        Ativo = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private void _UpdateDetails(string nome, Cpf cpf, DateTime dataNascimento)
    {
        Nome = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento.Date;
        UpdatedAt = DateTime.UtcNow;
    }
}
