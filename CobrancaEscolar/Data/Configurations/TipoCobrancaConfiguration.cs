using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class TipoCobrancaConfiguration : IEntityTypeConfiguration<TipoCobranca>
{
    public void Configure(EntityTypeBuilder<TipoCobranca> builder)
    {
        builder.ToTable("tipocobranca");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(t => t.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();

        builder.HasIndex(t => t.Nome).IsUnique().HasDatabaseName("uq_tipocobranca_nome");
    }
}
