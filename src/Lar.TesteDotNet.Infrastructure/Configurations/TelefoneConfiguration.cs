using Lar.TesteDotNet.Domain.Entities.Telefones;
using Lar.TesteDotNet.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lar.TesteDotNet.Infrastructure.Configurations;

public sealed class TelefoneConfiguration : IEntityTypeConfiguration<Telefone>
{
    public void Configure(EntityTypeBuilder<Telefone> builder)
    {
        builder.Setup("telefones");

        builder.Property(x => x.PessoaId).IsRequired();
        builder.Property(x => x.Numero).IsRequired().HasMaxLength(11);
        builder.Property(x => x.Tipo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(x => x.Pessoa)
            .WithMany(x => x.Telefones)
            .HasForeignKey(x => x.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.PessoaId, x.Numero }).IsUnique();
    }
}
