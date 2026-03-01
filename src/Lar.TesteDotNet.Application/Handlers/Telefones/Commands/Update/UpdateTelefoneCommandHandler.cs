using Lar.TesteDotNet.Domain.Entities.Telefones;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Update;

public sealed class UpdateTelefoneCommandHandler(IUnitOfWork uow)
    : ICommandHandler<UpdateTelefoneCommand, RequestResult>
{
    public async Task<RequestResult> Handle(UpdateTelefoneCommand command, CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Telefone>();

        var entity = await repo
            .FindBy(c => c.Id == command.Id && c.PessoaId == command.PessoaId)
            .FirstAsync(cancellationToken);

        var result = entity.UpdateDetails(command.Tipo, command.Numero);

        if (!result.IsSuccess) return RequestResult.Failure(result.Errors);

        await uow.CommitAsync(cancellationToken);

        return RequestResult.Success();
    }
}
