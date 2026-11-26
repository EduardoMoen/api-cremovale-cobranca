using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class AlineaConfiguration : IEntityTypeConfiguration<Alinea>
{
    public void Configure(EntityTypeBuilder<Alinea> builder)
    {
        builder.ToTable("alinea");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(a => a.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(a => a.DataUltimaAlteracao).HasColumnName("data_ultima_alteracao");

        builder.HasIndex(a => a.Nome).IsUnique().HasDatabaseName("uq_alinea_nome");
    }
}
