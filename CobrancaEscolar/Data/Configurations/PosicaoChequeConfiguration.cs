using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class PosicaoChequeConfiguration : IEntityTypeConfiguration<PosicaoCheque>
{
    public void Configure(EntityTypeBuilder<PosicaoCheque> builder)
    {
        builder.ToTable("posicaocheque");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(p => p.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(p => p.EscritorioId).HasColumnName("escritorio_id").IsRequired();

        builder.HasIndex(p => new { p.Nome, p.EscritorioId })
            .IsUnique().HasDatabaseName("uq_posicaocheque_nome_escritorio");
        builder.HasIndex(p => p.EscritorioId).HasDatabaseName("ix_posicaocheque_escritorio_id");

        builder.HasOne(p => p.Escritorio)
            .WithMany(e => e.PosicoesCheque)
            .HasForeignKey(p => p.EscritorioId)
            .HasConstraintName("fk_posicaocheque_escritorio")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
