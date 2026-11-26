using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class LugarConfiguration : IEntityTypeConfiguration<Lugar>
{
    public void Configure(EntityTypeBuilder<Lugar> builder)
    {
        builder.ToTable("lugar");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(l => l.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(l => l.EscritorioId).HasColumnName("escritorio_id").IsRequired();

        builder.HasIndex(l => new { l.Nome, l.EscritorioId })
            .IsUnique().HasDatabaseName("uq_lugar_nome_escritorio");
        builder.HasIndex(l => l.EscritorioId).HasDatabaseName("ix_lugar_escritorio_id");

        builder.HasOne(l => l.Escritorio)
            .WithMany(e => e.Lugares)
            .HasForeignKey(l => l.EscritorioId)
            .HasConstraintName("fk_lugar_escritorio")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
