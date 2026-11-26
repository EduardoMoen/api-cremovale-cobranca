namespace CobrancaEscolar.DTOs.Lugar;

public class LugarDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid EscritorioId { get; set; }
}

public class CreateLugarDto
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdateLugarDto
{
    public string Nome { get; set; } = string.Empty;
}

public class LugarFilterDto
{
    public string? Nome { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
