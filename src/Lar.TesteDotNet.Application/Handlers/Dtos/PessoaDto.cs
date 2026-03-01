using Mapster;
using Lar.TesteDotNet.Domain.Entities.Pessoas;

namespace Lar.TesteDotNet.Application.Handlers.Dtos;

public sealed class PessoaDto : IMapFrom<Pessoa>
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public bool Ativo { get; set; }
    public List<TelefoneDto> Telefones { get; set; } = [];

    public void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<Pessoa, PessoaDto>()
            .Map(dest => dest.Cpf, src => src.Cpf.ToString());
    }
}
