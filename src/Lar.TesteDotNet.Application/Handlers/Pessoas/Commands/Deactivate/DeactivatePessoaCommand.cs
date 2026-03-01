namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Deactivate;

public sealed class DeactivatePessoaCommand : ICommand<RequestResult>
{
    public long Id { get; set; }
}
