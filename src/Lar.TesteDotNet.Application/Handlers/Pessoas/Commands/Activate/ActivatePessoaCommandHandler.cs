using Lar.TesteDotNet.Domain.Entities.Pessoas;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Activate;

public sealed class ActivatePessoaCommandHandler(IUnitOfWork uow)
    : ICommandHandler<ActivatePessoaCommand, RequestResult>
{
    public async Task<RequestResult> Handle(ActivatePessoaCommand command, CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Pessoa>();

        var entity = await repo
            .FindBy(c => c.Id == command.Id)
            .FirstAsync(cancellationToken);

        var result = entity.Activate();

        if (!result.IsSuccess) return RequestResult.Failure(result.Errors);

        await uow.CommitAsync(cancellationToken);

        return RequestResult.Success();
    }
}
