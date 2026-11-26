using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class EscritorioConfiguration : IEntityTypeConfiguration<Escritorio>
{
    public void Configure(EntityTypeBuilder<Escritorio> builder)
    {
        builder.ToTable("escritorio");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
        builder.Property(e => e.Endereco).HasColumnName("endereco").HasMaxLength(255);
        builder.Property(e => e.Fone).HasColumnName("fone").HasMaxLength(50);

        builder.HasIndex(e => e.Nome).IsUnique().HasDatabaseName("uq_escritorio_nome");
    }
}
