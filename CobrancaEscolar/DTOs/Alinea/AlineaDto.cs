namespace CobrancaEscolar.DTOs.Alinea;

public class AlineaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime? DataUltimaAlteracao { get; set; }
}

public class CreateAlineaDto
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdateAlineaDto
{
    public string Nome { get; set; } = string.Empty;
}

public class AlineaFilterDto
{
    public string? Nome { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
