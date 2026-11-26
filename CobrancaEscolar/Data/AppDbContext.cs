using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Alinea> Alineas { get; set; }
    public DbSet<Banco> Bancos { get; set; }
    public DbSet<TipoCobranca> TiposCobranca { get; set; }
    public DbSet<Indice> Indices { get; set; }
    public DbSet<Escritorio> Escritorios { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Entidade> Entidades { get; set; }
    public DbSet<Escola> Escolas { get; set; }
    public DbSet<Responsavel> Responsaveis { get; set; }
    public DbSet<Andamento> Andamentos { get; set; }
    public DbSet<Lugar> Lugares { get; set; }
    public DbSet<PosicaoCheque> PosicoesCheque { get; set; }
    public DbSet<PosicaoContrato> PosicoesContrato { get; set; }
    public DbSet<Divida> Dividas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
