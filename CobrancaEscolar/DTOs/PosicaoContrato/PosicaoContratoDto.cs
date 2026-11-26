namespace CobrancaEscolar.DTOs.PosicaoContrato;

public class PosicaoContratoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid EscritorioId { get; set; }
}

public class CreatePosicaoContratoDto
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdatePosicaoContratoDto
{
    public string Nome { get; set; } = string.Empty;
}

public class PosicaoContratoFilterDto
{
    public string? Nome { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
