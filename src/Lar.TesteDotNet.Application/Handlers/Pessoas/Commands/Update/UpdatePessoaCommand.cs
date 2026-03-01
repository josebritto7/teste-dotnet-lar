namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Update;

public sealed class UpdatePessoaCommand : ICommand<RequestResult>
{
    [JsonIgnore] public long Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
}
