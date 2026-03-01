namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Activate;

public sealed class ActivatePessoaCommand : ICommand<RequestResult>
{
    public long Id { get; set; }
}
