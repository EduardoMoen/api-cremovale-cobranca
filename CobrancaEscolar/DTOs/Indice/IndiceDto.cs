namespace CobrancaEscolar.DTOs.Indice;

public class IndiceDto
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public decimal ValorIndice { get; set; }
}

public class CreateIndiceDto
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public decimal ValorIndice { get; set; }
}

public class UpdateIndiceDto
{
    public decimal ValorIndice { get; set; }
}

public class IndiceFilterDto
{
    public int? Ano { get; set; }
    public int? Mes { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
