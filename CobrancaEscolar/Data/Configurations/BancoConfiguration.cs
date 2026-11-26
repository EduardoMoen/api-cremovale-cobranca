using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class BancoConfiguration : IEntityTypeConfiguration<Banco>
{
    public void Configure(EntityTypeBuilder<Banco> builder)
    {
        builder.ToTable("banco");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(b => b.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();

        builder.HasIndex(b => b.Nome).IsUnique().HasDatabaseName("uq_banco_nome");
    }
}
