namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Delete;

public sealed class DeleteTelefoneCommand : ICommand<RequestResult>
{
    [JsonIgnore] public long Id { get; set; }
    [JsonIgnore] public long PessoaId { get; set; }
}
