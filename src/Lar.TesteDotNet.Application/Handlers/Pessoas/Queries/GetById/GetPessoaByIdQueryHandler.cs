using Lar.TesteDotNet.Application.Handlers.Dtos;
using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Shared.Interfaces.Database;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Queries.GetById;

public sealed class GetPessoaByIdQueryHandler(IUnitOfWork uow)
    : IQueryHandler<GetPessoaByIdQuery, RequestResult<PessoaDto>>
{
    public async Task<RequestResult<PessoaDto>> Handle(GetPessoaByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await uow.GetRepository<Pessoa>()
            .FindBy(c => c.Id == query.Id)
            .Include(x => x.Telefones)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return RequestResult<PessoaDto>.Success(entity?.Adapt<PessoaDto>() ?? new PessoaDto());
    }
}
