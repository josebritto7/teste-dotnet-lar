using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Shared.Interfaces.Database;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Application.Handlers.Pessoas.Commands.Create;

public sealed class CreatePessoaCommandHandler(IUnitOfWork uow)
    : ICommandHandler<CreatePessoaCommand, RequestResult<long>>
{
    public async Task<RequestResult<long>> Handle(
        CreatePessoaCommand command,
        CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Pessoa>();

        var entity = Pessoa.Create(command.Nome, command.Cpf, command.DataNascimento);

        if (!entity.IsSuccess) return RequestResult<long>.Failure(entity.Errors);

        repo.Add(entity.Value);

        await uow.CommitAsync(cancellationToken);

        return RequestResult<long>.Success(entity.Value.Id);
    }
}
