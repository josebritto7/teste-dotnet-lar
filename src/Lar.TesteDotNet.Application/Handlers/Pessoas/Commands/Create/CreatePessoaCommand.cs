using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Create;

public sealed class CreatePessoaCommand : ICommand<RequestResult<long>>
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
}
