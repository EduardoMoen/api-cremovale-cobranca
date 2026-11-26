namespace CobrancaEscolar.Common;

/// <summary>
/// Utilitário para hash de senhas usando BCrypt.
/// A senha é armazenada como hash — nunca em texto puro.
/// </summary>
public static class PasswordHelper
{
    private const int WorkFactor = 12;

    /// <summary>Senha vazia (marcador de primeiro acesso sem senha definida).</summary>
    public const string NoPasswordHash = "";

    public static string Hash(string plainPassword)
        => BCrypt.Net.BCrypt.HashPassword(plainPassword, WorkFactor);

    public static bool Verify(string plainPassword, string hash)
        => BCrypt.Net.BCrypt.Verify(plainPassword, hash);

    public static bool HasPassword(string? hash)
        => !string.IsNullOrWhiteSpace(hash);
}
