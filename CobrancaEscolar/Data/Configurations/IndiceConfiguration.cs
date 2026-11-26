using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class IndiceConfiguration : IEntityTypeConfiguration<Indice>
{
    public void Configure(EntityTypeBuilder<Indice> builder)
    {
        builder.ToTable("indice");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(i => i.Ano).HasColumnName("ano").IsRequired();
        builder.Property(i => i.Mes).HasColumnName("mes").IsRequired();
        builder.Property(i => i.ValorIndice).HasColumnName("indice").HasColumnType("numeric(16,8)").IsRequired();

        builder.HasIndex(i => new { i.Ano, i.Mes }).IsUnique().HasDatabaseName("uq_indice_ano_mes");
    }
}
