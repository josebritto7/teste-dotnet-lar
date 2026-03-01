using Lar.TesteDotNet.Domain.Entities.Pessoas;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Update;

public sealed class UpdatePessoaCommandHandler(IUnitOfWork uow)
    : ICommandHandler<UpdatePessoaCommand, RequestResult>
{
    public async Task<RequestResult> Handle(UpdatePessoaCommand command, CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Pessoa>();

        var entity = await repo
            .FindBy(c => c.Id == command.Id)
            .FirstAsync(cancellationToken);

        var result = entity.UpdateDetails(command.Nome, command.Cpf, command.DataNascimento);

        if (!result.IsSuccess) return RequestResult.Failure(result.Errors);

        await uow.CommitAsync(cancellationToken);

        return RequestResult.Success();
    }
}
