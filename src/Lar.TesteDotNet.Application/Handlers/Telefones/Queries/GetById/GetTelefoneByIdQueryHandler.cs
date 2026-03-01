using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Shared.Interfaces.Database;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;
using Mapster;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Queries.GetById;

public sealed class GetTelefoneByIdQueryHandler(IUnitOfWork uow)
    : IQueryHandler<GetTelefoneByIdQuery, RequestResult<TelefoneDto>>
{
    public async Task<RequestResult<TelefoneDto>> Handle(GetTelefoneByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await uow.GetRepository<Telefone>()
            .FindBy(c => c.PessoaId == query.PessoaId && c.Id == query.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return RequestResult<TelefoneDto>.Success(entity?.Adapt<TelefoneDto>() ?? new TelefoneDto());
    }
}
