namespace CobrancaEscolar.DTOs.Responsavel;

public class ResponsavelDto
{
    public Guid Id { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateOnly? Nascimento { get; set; }
    public string? Endereco { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }
    public string? Cep { get; set; }
    public string? Rg { get; set; }
    public DateOnly? RgEmissao { get; set; }
    public string? EstadoCivil { get; set; }
    public string? Telefones { get; set; }
    public string? Email { get; set; }
    public Guid EntidadeId { get; set; }
    public string? EntidadeNome { get; set; }
    public DateTime? UltimaAlteracao { get; set; }
}

public class CreateResponsavelDto
{
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateOnly? Nascimento { get; set; }
    public string? Endereco { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }
    public string? Cep { get; set; }
    public string? Rg { get; set; }
    public DateOnly? RgEmissao { get; set; }
    public string? EstadoCivil { get; set; }
    public string? Telefones { get; set; }
    public string? Email { get; set; }
    public Guid EntidadeId { get; set; }
}

public class UpdateResponsavelDto
{
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateOnly? Nascimento { get; set; }
    public string? Endereco { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }
    public string? Cep { get; set; }
    public string? Rg { get; set; }
    public DateOnly? RgEmissao { get; set; }
    public string? EstadoCivil { get; set; }
    public string? Telefones { get; set; }
    public string? Email { get; set; }
    public Guid EntidadeId { get; set; }
}

/// <summary>Resultado enxuto para campos de autocomplete.</summary>
public class ResponsavelAutocompleteDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}

public class ResponsavelFilterDto
{
    public Guid? EntidadeId { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
