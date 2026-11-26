using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class ResponsavelConfiguration : IEntityTypeConfiguration<Responsavel>
{
    public void Configure(EntityTypeBuilder<Responsavel> builder)
    {
        builder.ToTable("responsavel");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(r => r.Cpf).HasColumnName("cpf").HasMaxLength(14).IsRequired();
        builder.Property(r => r.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
        builder.Property(r => r.Nascimento).HasColumnName("nascimento");
        builder.Property(r => r.Endereco).HasColumnName("endereco").HasMaxLength(255);
        builder.Property(r => r.Complemento).HasColumnName("complemento").HasMaxLength(255);
        builder.Property(r => r.Bairro).HasColumnName("bairro").HasMaxLength(255);
        builder.Property(r => r.Cidade).HasColumnName("cidade").HasMaxLength(255);
        builder.Property(r => r.Uf).HasColumnName("uf").HasMaxLength(2);
        builder.Property(r => r.Cep).HasColumnName("cep").HasMaxLength(9);
        builder.Property(r => r.Rg).HasColumnName("rg").HasMaxLength(20);
        builder.Property(r => r.RgEmissao).HasColumnName("rg_emissao");
        builder.Property(r => r.EstadoCivil).HasColumnName("estado_civil").HasMaxLength(50);
        builder.Property(r => r.Telefones).HasColumnName("telefones").HasMaxLength(255);
        builder.Property(r => r.Email).HasColumnName("email").HasMaxLength(255);
        builder.Property(r => r.EntidadeId).HasColumnName("entidade_id").IsRequired();
        builder.Property(r => r.UltimaAlteracao).HasColumnName("ultima_alteracao")
            .HasDefaultValueSql("SYSDATETIME()");

        builder.HasIndex(r => new { r.Cpf, r.EntidadeId })
            .IsUnique().HasDatabaseName("uq_responsavel_cpf_entidade");
        builder.HasIndex(r => r.EntidadeId).HasDatabaseName("ix_responsavel_entidade_id");
        builder.HasIndex(r => r.Cpf).HasDatabaseName("ix_responsavel_cpf");
        builder.HasIndex(r => r.Nome).HasDatabaseName("ix_responsavel_nome");

        builder.HasOne(r => r.Entidade)
            .WithMany(e => e.Responsaveis)
            .HasForeignKey(r => r.EntidadeId)
            .HasConstraintName("fk_responsavel_entidade")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
