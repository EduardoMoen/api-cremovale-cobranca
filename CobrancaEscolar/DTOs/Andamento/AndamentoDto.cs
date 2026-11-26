namespace CobrancaEscolar.DTOs.Andamento;

public class AndamentoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid EscritorioId { get; set; }
}

public class CreateAndamentoDto
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdateAndamentoDto
{
    public string Nome { get; set; } = string.Empty;
}

public class AndamentoFilterDto
{
    public string? Nome { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
