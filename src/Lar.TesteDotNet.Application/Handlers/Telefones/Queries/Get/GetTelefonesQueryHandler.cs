using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Application.Pagination;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Shared.Interfaces.Database;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;
using Mapster;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Queries.Get;

public sealed class GetTelefonesQueryHandler(IUnitOfWork uow)
    : IQueryHandler<GetTelefonesQuery, RequestResult<PagedList<TelefoneDto>>>
{
    public async Task<RequestResult<PagedList<TelefoneDto>>> Handle(GetTelefonesQuery query,
        CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Telefone>();

        var decodedCursor = string.IsNullOrEmpty(query.Cursor)
            ? null
            : Cursor.Decode(query.Cursor);

        var baseQuery = decodedCursor is null
            ? repo.GetAllWithLimit(query.Limit, p => p.PessoaId == query.PessoaId)
            : repo.GetWithCursorFiltering(decodedCursor.Date,
                decodedCursor.Id,
                query.Limit,
                p => p.PessoaId == query.PessoaId);

        var entities = await baseQuery
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

        var items = entities.Adapt<List<TelefoneDto>>();

        return RequestResult<PagedList<TelefoneDto>>.Success(PagedList<TelefoneDto>
            .Create(items,
                Cursor.Encode(nextDate, nextId),
                hasNext));
    }
}
