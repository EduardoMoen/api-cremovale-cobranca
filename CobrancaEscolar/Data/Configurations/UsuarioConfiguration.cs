using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id").UseIdentityColumn();
        builder.Property(u => u.Username).HasColumnName("username").HasMaxLength(150).IsRequired();
        builder.Property(u => u.Password).HasColumnName("password").HasMaxLength(128).IsRequired();
        builder.Property(u => u.FirstName).HasColumnName("first_name").HasMaxLength(150).IsRequired();
        builder.Property(u => u.LastName).HasColumnName("last_name").HasMaxLength(150).IsRequired();
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(254).IsRequired();
        builder.Property(u => u.IsSuperuser).HasColumnName("is_superuser").HasDefaultValue(false);
        builder.Property(u => u.IsStaff).HasColumnName("is_staff").HasDefaultValue(false);
        builder.Property(u => u.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(u => u.DateJoined).HasColumnName("date_joined").HasDefaultValueSql("SYSDATETIME()");
        builder.Property(u => u.LastLogin).HasColumnName("last_login");
        builder.Property(u => u.EscritorioId).HasColumnName("escritorio_id");

        builder.HasIndex(u => u.Username).IsUnique().HasDatabaseName("uq_usuario_username");
        builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("uq_usuario_email");
        builder.HasIndex(u => u.EscritorioId).HasDatabaseName("ix_usuario_escritorio_id");

        builder.HasOne(u => u.Escritorio)
            .WithMany(e => e.Usuarios)
            .HasForeignKey(u => u.EscritorioId)
            .HasConstraintName("fk_usuario_escritorio")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
