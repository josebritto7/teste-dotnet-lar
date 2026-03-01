using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Queries.GetById;

public record GetPessoaByIdQuery(long Id) : IQuery<RequestResult<PessoaDto>>;
