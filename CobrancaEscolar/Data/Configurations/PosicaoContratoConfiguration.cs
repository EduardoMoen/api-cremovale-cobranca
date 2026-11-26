using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class PosicaoContratoConfiguration : IEntityTypeConfiguration<PosicaoContrato>
{
    public void Configure(EntityTypeBuilder<PosicaoContrato> builder)
    {
        builder.ToTable("posicaocontrato");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(p => p.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(p => p.EscritorioId).HasColumnName("escritorio_id").IsRequired();

        builder.HasIndex(p => new { p.Nome, p.EscritorioId })
            .IsUnique().HasDatabaseName("uq_posicaocontrato_nome_escritorio");
        builder.HasIndex(p => p.EscritorioId).HasDatabaseName("ix_posicaocontrato_escritorio_id");

        builder.HasOne(p => p.Escritorio)
            .WithMany(e => e.PosicoesContrato)
            .HasForeignKey(p => p.EscritorioId)
            .HasConstraintName("fk_posicaocontrato_escritorio")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
