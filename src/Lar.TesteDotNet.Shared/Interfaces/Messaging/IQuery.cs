namespace Lar.TesteDotNet.Shared.Interfaces.Messaging;

public interface IQuery<out TResponse>
{
}

public interface IQuery : IQuery<Unit>
{
}