namespace CobrancaEscolar.DTOs.Escritorio;

public class EscritorioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public string? Fone { get; set; }
}

public class CreateEscritorioDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public string? Fone { get; set; }
}

public class UpdateEscritorioDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public string? Fone { get; set; }
}

public class EscritorioFilterDto
{
    public string? Nome { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
