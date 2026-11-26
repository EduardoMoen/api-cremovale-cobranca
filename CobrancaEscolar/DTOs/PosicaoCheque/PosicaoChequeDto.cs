namespace CobrancaEscolar.DTOs.PosicaoCheque;

public class PosicaoChequeDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid EscritorioId { get; set; }
}

public class CreatePosicaoChequeDto
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdatePosicaoChequeDto
{
    public string Nome { get; set; } = string.Empty;
}

public class PosicaoChequeFilterDto
{
    public string? Nome { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
