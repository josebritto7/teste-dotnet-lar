using Lar.TesteDotNet.Domain.Enums;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Update;

public sealed class UpdateTelefoneCommand : ICommand<RequestResult>
{
    [JsonIgnore] public long Id { get; set; }
    [JsonIgnore] public long PessoaId { get; set; }

    public TipoTelefone Tipo { get; set; }
    public string Numero { get; set; } = string.Empty;
}
