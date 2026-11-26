namespace CobrancaEscolar.Entities;

public class Escritorio
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public string? Fone { get; set; }

    // Navegação
    public ICollection<Usuario> Usuarios { get; set; } = [];
    public ICollection<Entidade> Entidades { get; set; } = [];
    public ICollection<Andamento> Andamentos { get; set; } = [];
    public ICollection<Lugar> Lugares { get; set; } = [];
    public ICollection<PosicaoCheque> PosicoesCheque { get; set; } = [];
    public ICollection<PosicaoContrato> PosicoesContrato { get; set; } = [];
}
