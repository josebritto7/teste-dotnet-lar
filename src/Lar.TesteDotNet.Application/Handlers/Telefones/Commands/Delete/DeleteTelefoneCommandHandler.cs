using Lar.TesteDotNet.Domain.Entities.Telefones;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Delete;

public sealed class DeleteTelefoneCommandHandler(IUnitOfWork uow)
    : ICommandHandler<DeleteTelefoneCommand, RequestResult>
{
    public async Task<RequestResult> Handle(DeleteTelefoneCommand command, CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Telefone>();

        var entity = await repo
            .FindBy(c => c.Id == command.Id && c.PessoaId == command.PessoaId)
            .FirstAsync(cancellationToken);

        repo.Delete(entity);

        await uow.CommitAsync(cancellationToken);

        return RequestResult.Success();
    }
}
