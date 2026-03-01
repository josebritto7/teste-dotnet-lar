using Lar.TesteDotNet.Domain.Enums;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Create;

public sealed class CreateTelefoneCommand : ICommand<RequestResult<long>>
{
    [JsonIgnore] public long PessoaId { get; set; }

    public TipoTelefone Tipo { get; set; }
    public string Numero { get; set; } = string.Empty;
}
