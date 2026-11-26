using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class AndamentoConfiguration : IEntityTypeConfiguration<Andamento>
{
    public void Configure(EntityTypeBuilder<Andamento> builder)
    {
        builder.ToTable("andamento");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(a => a.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(a => a.EscritorioId).HasColumnName("escritorio_id").IsRequired();

        builder.HasIndex(a => new { a.Nome, a.EscritorioId })
            .IsUnique().HasDatabaseName("uq_andamento_nome_escritorio");
        builder.HasIndex(a => a.EscritorioId).HasDatabaseName("ix_andamento_escritorio_id");

        builder.HasOne(a => a.Escritorio)
            .WithMany(e => e.Andamentos)
            .HasForeignKey(a => a.EscritorioId)
            .HasConstraintName("fk_andamento_escritorio")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
