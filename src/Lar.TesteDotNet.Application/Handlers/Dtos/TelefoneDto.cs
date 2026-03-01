using Mapster;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Domain.Enums;

namespace Lar.TesteDotNet.Application.Handlers.Dtos;

public sealed class TelefoneDto : IMapFrom<Telefone>
{
    public long Id { get; set; }
    public long PessoaId { get; set; }
    public TipoTelefone Tipo { get; set; }
    public string Numero { get; set; } = string.Empty;

    public void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<Telefone, TelefoneDto>();
    }
}
