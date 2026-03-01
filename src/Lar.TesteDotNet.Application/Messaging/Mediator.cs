using Microsoft.Extensions.DependencyInjection;
using Lar.TesteDotNet.Shared.Interfaces.Messaging;

namespace Lar.TesteDotNet.Application.Messaging;

public class Mediator(IServiceProvider provider) : IMediator
{
    public Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResult>
    {
        var handler = provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return handler.Handle(command, cancellationToken);
    }

    public Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>
    {
        var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return handler.Handle(query, cancellationToken);
    }
}