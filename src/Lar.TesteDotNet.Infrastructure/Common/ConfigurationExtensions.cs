using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Lar.TesteDotNet.Domain.Common;

namespace Lar.TesteDotNet.Infrastructure.Common;

public static class ConfigurationExtensions
{
    public static EntityTypeBuilder<T> Setup<T>(
        this EntityTypeBuilder<T> builder,
        string tableName) where T : Entity
    {
        builder.ToTable(tableName);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).HasDefaultValue(null);

        return builder;
    }
}