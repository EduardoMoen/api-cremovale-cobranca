namespace CobrancaEscolar.Entities;

public class PosicaoCheque
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public Guid EscritorioId { get; set; }

    // Navegação
    public Escritorio? Escritorio { get; set; }
}
