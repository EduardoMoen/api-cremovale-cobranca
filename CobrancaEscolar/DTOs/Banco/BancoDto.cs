namespace CobrancaEscolar.DTOs.Banco;

public class BancoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class CreateBancoDto
{
    public string Nome { get; set; } = string.Empty;
}

public class UpdateBancoDto
{
    public string Nome { get; set; } = string.Empty;
}

public class BancoFilterDto
{
    public string? Nome { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
