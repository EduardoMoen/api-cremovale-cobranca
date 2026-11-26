namespace CobrancaEscolar.DTOs.Entidade;

public class EntidadeDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public Guid EscritorioId { get; set; }
    public string? EscritorioNome { get; set; }
}

public class CreateEntidadeDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    // EscritorioId é preenchido automaticamente pelo escritório do usuário autenticado
}

public class UpdateEntidadeDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}

public class EntidadeFilterDto
{
    public string? Nome { get; set; }
    public string? Codigo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
