using Lar.TesteDotNet.Domain.Entities.Pessoas;
using Lar.TesteDotNet.Domain.ValueObjects;
using Lar.TesteDotNet.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lar.TesteDotNet.Infrastructure.Configurations;

public sealed class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.Setup("pessoas");

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Cpf)
            .HasConversion(x => x.ToString(), v => Cpf.Create(v).Value)
            .IsRequired()
            .HasMaxLength(11);
        builder.Property(x => x.DataNascimento).IsRequired();
        builder.Property(x => x.Ativo).IsRequired();

        builder.HasIndex(x => x.Cpf).IsUnique();
    }
}
