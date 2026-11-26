namespace CobrancaEscolar.DTOs.Escola;

public class EscolaDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }
    public string? Cep { get; set; }
    public string? Email { get; set; }
    public string? Obs { get; set; }
    public Guid EntidadeId { get; set; }
    public string? EntidadeNome { get; set; }
}

public class CreateEscolaDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }
    public string? Cep { get; set; }
    public string? Email { get; set; }
    public string? Obs { get; set; }
    public Guid EntidadeId { get; set; }
}

public class UpdateEscolaDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }
    public string? Cep { get; set; }
    public string? Email { get; set; }
    public string? Obs { get; set; }
    public Guid EntidadeId { get; set; }
}

public class EscolaFilterDto
{
    public string? Nome { get; set; }
    public string? Codigo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
