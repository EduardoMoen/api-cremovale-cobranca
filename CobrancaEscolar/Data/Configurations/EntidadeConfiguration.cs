using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class EntidadeConfiguration : IEntityTypeConfiguration<Entidade>
{
    public void Configure(EntityTypeBuilder<Entidade> builder)
    {
        builder.ToTable("entidade");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(255).IsRequired();
        builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(e => e.EscritorioId).HasColumnName("escritorio_id").IsRequired();

        builder.HasIndex(e => new { e.Codigo, e.EscritorioId })
            .IsUnique().HasDatabaseName("uq_entidade_codigo_escritorio");
        builder.HasIndex(e => e.EscritorioId).HasDatabaseName("ix_entidade_escritorio_id");

        builder.HasOne(e => e.Escritorio)
            .WithMany(es => es.Entidades)
            .HasForeignKey(e => e.EscritorioId)
            .HasConstraintName("fk_entidade_escritorio")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
