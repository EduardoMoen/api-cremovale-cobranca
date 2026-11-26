namespace CobrancaEscolar.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    /// <summary>Hash BCrypt da senha. Nunca armazenar texto puro.</summary>
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsSuperuser { get; set; }
    public bool IsStaff { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    public Guid? EscritorioId { get; set; }

    // Navegação
    public Escritorio? Escritorio { get; set; }
}
