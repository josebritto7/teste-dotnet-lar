using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Application.Pagination;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Queries.Get;

public sealed class GetTelefonesQuery : PaginatedQuery, IQuery<RequestResult<PagedList<TelefoneDto>>>
{
    public long PessoaId { get; init; }
}
