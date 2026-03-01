namespace Lar.TesteDotNet.Domain.Common;

public abstract class Entity
{
    public long Id { get; set; }
    
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
}