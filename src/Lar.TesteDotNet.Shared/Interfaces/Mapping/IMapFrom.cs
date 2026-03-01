using Mapster;

namespace Lar.TesteDotNet.Shared.Interfaces.Mapping;

public interface IMapFrom<T>
{
    void Mapping(TypeAdapterConfig config);
}