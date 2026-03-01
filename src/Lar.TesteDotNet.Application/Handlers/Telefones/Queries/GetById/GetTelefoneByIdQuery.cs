using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Queries.GetById;

public record GetTelefoneByIdQuery(long PessoaId, long Id) : IQuery<RequestResult<TelefoneDto>>;
