using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Shared.Interfaces.Database;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;
using Lar.TesteDotNet.Shared.Wrappers;

namespace Lar.TesteDotNet.Application.Handlers.Telefones.Commands.Create;

public sealed class CreateTelefoneCommandHandler(IUnitOfWork uow)
    : ICommandHandler<CreateTelefoneCommand, RequestResult<long>>
{
    public async Task<RequestResult<long>> Handle(
        CreateTelefoneCommand command,
        CancellationToken cancellationToken)
    {
        var repo = uow.GetRepository<Telefone>();

        var entity = Telefone.Create(command.PessoaId, command.Tipo, command.Numero);

        if (!entity.IsSuccess) return RequestResult<long>.Failure(entity.Errors);

        repo.Add(entity.Value);

        await uow.CommitAsync(cancellationToken);

        return RequestResult<long>.Success(entity.Value.Id);
    }
}
