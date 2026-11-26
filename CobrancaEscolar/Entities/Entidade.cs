namespace CobrancaEscolar.Entities;

public class Entidade
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public Guid EscritorioId { get; set; }

    // Navegação
    public Escritorio? Escritorio { get; set; }
    public ICollection<Escola> Escolas { get; set; } = [];
    public ICollection<Responsavel> Responsaveis { get; set; } = [];
}
