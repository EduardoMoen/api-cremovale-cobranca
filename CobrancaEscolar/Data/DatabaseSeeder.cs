using CobrancaEscolar.Common;
using CobrancaEscolar.Data;
using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Data;

/// <summary>
/// Executa seed inicial do banco de dados.
/// Cria o superusuário padrão se não existir nenhum usuário cadastrado.
/// </summary>
public static class DatabaseSeeder
{
    private const string DefaultUsername = "admin";
    private const string DefaultPassword = "Admin@123";

    public static async Task SeedAsync(IServiceProvider services, ILogger logger)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            // Aguarda o banco estar acessível
            await db.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Seed ignorado: não foi possível conectar ao banco de dados.");
            return;
        }

        // Só cria o admin se não existir nenhum usuário no banco
        if (await db.Usuarios.AnyAsync())
        {
            logger.LogDebug("Seed ignorado: já existem usuários cadastrados.");
            return;
        }

        var admin = new Usuario
        {
            Username    = DefaultUsername,
            Password    = PasswordHelper.Hash(DefaultPassword),
            FirstName   = "Administrador",
            LastName    = "Sistema",
            Email       = "admin@sistema.local",
            IsSuperuser = true,
            IsStaff     = true,
            IsActive    = true,
            DateJoined  = DateTime.UtcNow,
            EscritorioId = null   // SuperUser não precisa de escritório
        };

        db.Usuarios.Add(admin);
        await db.SaveChangesAsync();

        logger.LogWarning(
            ">>> SEED: Superusuário '{Username}' criado com senha padrão '{Password}'. " +
            "Troque a senha imediatamente após o primeiro login! <<<",
            DefaultUsername, DefaultPassword);
    }
}
