using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Application.Pagination;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Shared.Interfaces.Database;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Queries.Get;

public sealed class GetPessoasQueryHandler(IUnitOfWork uow)
    : IQueryHandler<GetPessoasQuery, RequestResult<PagedList<PessoaDto>>>
{
    public async Task<RequestResult<PagedList<PessoaDto>>> Handle(GetPessoasQuery query,
        CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Pessoa>();

        var decodedCursor = string.IsNullOrEmpty(query.Cursor)
            ? null
            : Cursor.Decode(query.Cursor);

        var baseQuery = decodedCursor is null
            ? repo.GetAllWithLimit(query.Limit)
            : repo.GetWithCursorFiltering(decodedCursor.Date, decodedCursor.Id, query.Limit);

        var entities = await baseQuery
            .Include(x => x.Telefones)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var hasNext = entities.Count > query.Limit;

        DateTime? nextDate = null;
        long? nextId = null;
        if (entities.Count > 0)
        {
            var cursorItem = hasNext ? entities[query.Limit - 1] : entities[^1];
            nextDate = hasNext ? cursorItem.CreatedAt : null;
            nextId = hasNext ? cursorItem.Id : null;
        }

        if (hasNext)
            entities.RemoveAt(entities.Count - 1);

        var items = entities.Adapt<List<PessoaDto>>();

        return RequestResult<PagedList<PessoaDto>>.Success(PagedList<PessoaDto>
            .Create(items,
                Cursor.Encode(nextDate, nextId),
                hasNext));
    }
}
