namespace CobrancaEscolar.DTOs.Usuario;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsSuperuser { get; set; }
    public bool IsStaff { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateJoined { get; set; }
    public DateTime? LastLogin { get; set; }
    public Guid? EscritorioId { get; set; }
    public string? EscritorioNome { get; set; }
}

public class CreateUsuarioDto
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsSuperuser { get; set; }
    public bool IsStaff { get; set; }
    public Guid? EscritorioId { get; set; }
}

public class UpdateUsuarioDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsSuperuser { get; set; }
    public bool IsStaff { get; set; }
    public bool IsActive { get; set; }
    public Guid? EscritorioId { get; set; }
}

public class UsuarioFilterDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
