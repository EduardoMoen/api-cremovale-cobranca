namespace CobrancaEscolar.DTOs.TipoCobranca;

public class TipoCobrancaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class CreateTipoCobrancaDto
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdateTipoCobrancaDto
{
    public string Nome { get; set; } = string.Empty;
}

public class TipoCobrancaFilterDto
{
    public string? Nome { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
