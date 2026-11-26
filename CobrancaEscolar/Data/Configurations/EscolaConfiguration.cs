using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class EscolaConfiguration : IEntityTypeConfiguration<Escola>
{
    public void Configure(EntityTypeBuilder<Escola> builder)
    {
        builder.ToTable("escola");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(255).IsRequired();
        builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(e => e.Endereco).HasColumnName("endereco").HasMaxLength(255);
        builder.Property(e => e.Bairro).HasColumnName("bairro").HasMaxLength(255);
        builder.Property(e => e.Cidade).HasColumnName("cidade").HasMaxLength(255);
        builder.Property(e => e.Uf).HasColumnName("uf").HasMaxLength(2);
        builder.Property(e => e.Cep).HasColumnName("cep").HasMaxLength(9);
        builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
        builder.Property(e => e.Obs).HasColumnName("obs").HasMaxLength(255);
        builder.Property(e => e.EntidadeId).HasColumnName("entidade_id").IsRequired();

        builder.HasIndex(e => new { e.Codigo, e.EntidadeId })
            .IsUnique().HasDatabaseName("uq_escola_codigo_entidade");
        builder.HasIndex(e => e.EntidadeId).HasDatabaseName("ix_escola_entidade_id");
        builder.HasIndex(e => e.Nome).HasDatabaseName("ix_escola_nome");

        builder.HasOne(e => e.Entidade)
            .WithMany(en => en.Escolas)
            .HasForeignKey(e => e.EntidadeId)
            .HasConstraintName("fk_escola_entidade")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
