namespace CobrancaEscolar.Entities;

public class PosicaoContrato
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public Guid EscritorioId { get; set; }

    // Navegação
    public Escritorio? Escritorio { get; set; }
}
