using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Application.Pagination;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Queries.Get;

public sealed class GetPessoasQuery : PaginatedQuery, IQuery<RequestResult<PagedList<PessoaDto>>>
{
}
