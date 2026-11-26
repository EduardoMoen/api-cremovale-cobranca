namespace CobrancaEscolar.Entities;

public class Responsavel
{
    public Guid Id { get; set; } = Guid.NewGuid();
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
    public DateTime? UltimaAlteracao { get; set; }

    // Navegação
    public Entidade? Entidade { get; set; }
}
