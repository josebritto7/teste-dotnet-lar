using Lar.TesteDotNet.Domain.Entities.Pessoas;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Deactivate;

public sealed class DeactivatePessoaCommandHandler(IUnitOfWork uow)
    : ICommandHandler<DeactivatePessoaCommand, RequestResult>
{
    public async Task<RequestResult> Handle(DeactivatePessoaCommand command, CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Pessoa>();

        var entity = await repo
            .FindBy(c => c.Id == command.Id)
            .FirstAsync(cancellationToken);

        var result = entity.Deactivate();

        if (!result.IsSuccess) return RequestResult.Failure(result.Errors);

        await uow.CommitAsync(cancellationToken);

        return RequestResult.Success();
    }
}
