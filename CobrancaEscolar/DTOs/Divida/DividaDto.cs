namespace CobrancaEscolar.DTOs.Divida;

/// <summary>
/// Campos editáveis da dívida. Usado na criação e na alteração.
/// Campos somente leitura (importação, carta, alterado_em/por) ficam fora.
/// </summary>
public class DividaSaveDto
{
    // ─── Aba: Dívida ────────────────────────────────────────
    public Guid EntidadeId { get; set; }
    public Guid ResponsavelId { get; set; }
    public int TipoCobrancaId { get; set; }
    public string NumeroCobranca { get; set; } = string.Empty;
    public int? BancoId { get; set; }
    public string? Agencia { get; set; }
    public string? Conta { get; set; }
    public int? AlineaId { get; set; }
    public DateOnly DataVencimento { get; set; }
    public decimal ValorCobranca { get; set; }
    public decimal? ValorCobrancaAcao { get; set; }
    public bool? StatusValorAcao { get; set; }
    public string? Parcela { get; set; }
    public decimal? ValorMulta { get; set; }
    public decimal? ValorJuro { get; set; }
    public DateOnly? DataAcertoMantenedora { get; set; }
    public decimal? ValorPago { get; set; }
    public string? NumeroContrato { get; set; }
    public Guid? ResponsavelAtualId { get; set; }
    public bool? StatusBolsista { get; set; }
    public bool? StatusEstornado { get; set; }
    public int? CodigoAluno { get; set; }
    public string? NomeAluno { get; set; }
    public string? Serie { get; set; }
    public int? Ano { get; set; }
    public Guid? EscolaId { get; set; }

    // ─── Aba: Cálculo ───────────────────────────────────────
    public DateOnly? DataInicioJuro { get; set; }
    public DateOnly? DataAcertoJw { get; set; }
    public int? NumeroDias { get; set; }
    public int? MesInicioCorrecao { get; set; }
    public int? AnoInicioCorrecao { get; set; }
    public decimal? IndiceInicial { get; set; }
    public int? MesFimCorrecao { get; set; }
    public int? AnoFimCorrecao { get; set; }
    public decimal? IndiceFinal { get; set; }
    public decimal? ValorCorrigido { get; set; }
    public decimal? PercentualMulta { get; set; }
    public decimal? ValorMultaJw { get; set; }
    public decimal? PercentualJuros { get; set; }
    public decimal? ValorJuroJw { get; set; }
    public decimal? ValorSubtotal { get; set; }
    public decimal? PercentualHonorarios { get; set; }
    public decimal? ValorHonorarios { get; set; }
    public decimal? ValorTotal { get; set; }
    public bool? StatusPagoJw { get; set; }
    public DateOnly? DataPagoJw { get; set; }
    public decimal? ValorCobrado { get; set; }

    // ─── Aba: Complemento ───────────────────────────────────
    public int? NumeroRecibo { get; set; }
    public string? NomeResponsavelCobranca { get; set; }
    public Guid? PosicaoContratoId { get; set; }
    public Guid? PosicaoChequeId { get; set; }
    public Guid? AndamentoId { get; set; }
    public Guid? LugarId { get; set; }
    public bool? StatusAcao { get; set; }
    public bool? StatusSpcNaoEnviar { get; set; }
    public bool? StatusNaoEnviarCarta { get; set; }
    public bool? StatusAcordo { get; set; }
    public bool? StatusSpcBaixaManual { get; set; }
    public bool? StatusSpcBaixar { get; set; }
    public string? Pasta { get; set; }
    public string? NumeroVara { get; set; }
    public string? NumeroProcesso { get; set; }
    public DateOnly? DataAcao { get; set; }
    public DateOnly? DataSpcEnvio { get; set; }
    public string? StatusSpcEnvio { get; set; }
    public DateOnly? DataSpcBaixa { get; set; }
    public string? StatusSpcBaixa { get; set; }
    public int? RemessaSpcEnvio { get; set; }
    public int? RemessaSpcBaixa { get; set; }
    public DateOnly? DataAcertoJwMantenedora { get; set; }

    // ─── Aba: Observações ───────────────────────────────────
    public string? ObsJw { get; set; }
}

/// <summary>Dívida completa (detalhe), com campos somente leitura e nomes para exibição.</summary>
public class DividaDto : DividaSaveDto
{
    public Guid Id { get; set; }

    // Somente leitura
    public DateOnly? DataPrimeiraImportacao { get; set; }
    public DateOnly? DataUltimaImportacao { get; set; }
    public DateOnly? DataImpressaoCarta { get; set; }
    public DateTime AlteradoEm { get; set; }
    public int? AlteradoPorId { get; set; }
    public string? AlteradoPorNome { get; set; }

    // Rótulos para os campos de autocomplete
    public string? ResponsavelNome { get; set; }
    public string? ResponsavelCpf { get; set; }
    public string? ResponsavelAtualNome { get; set; }
    public string? ResponsavelAtualCpf { get; set; }
}

/// <summary>Linha enxuta do grid de consulta.</summary>
public class DividaListItemDto
{
    public Guid Id { get; set; }
    public string EntidadeNome { get; set; } = string.Empty;
    public string ResponsavelNome { get; set; } = string.Empty;
    public string TipoCobrancaNome { get; set; } = string.Empty;
    public string NumeroCobranca { get; set; } = string.Empty;
    public string? Parcela { get; set; }
    public DateOnly DataVencimento { get; set; }
    public decimal ValorCobranca { get; set; }
    public string? NomeAluno { get; set; }
}

public class DividaFilterDto
{
    public Guid? EntidadeId { get; set; }
    public Guid? ResponsavelId { get; set; }
    public int? TipoCobrancaId { get; set; }
    public string? NumeroCobranca { get; set; }
    public string? NomeAluno { get; set; }
    public DateOnly? VencimentoInicial { get; set; }
    public DateOnly? VencimentoFinal { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
