namespace CobrancaEscolar.Entities;

public class Escola
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }
    public string? Cep { get; set; }
    public string? Email { get; set; }
    public string? Obs { get; set; }
    public Guid EntidadeId { get; set; }

    // Navegação
    public Entidade? Entidade { get; set; }
}
