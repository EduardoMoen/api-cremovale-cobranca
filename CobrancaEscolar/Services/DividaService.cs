using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Divida;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class DividaService(AppDbContext db) : IDividaService
{
    private const int MaxPageSize = 100;

    public async Task<PagedResultDto<DividaListItemDto>> GetAllAsync(DividaFilterDto filter, Guid escritorioId)
    {
        var page = Math.Max(filter.Page, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, MaxPageSize);

        // Multitenancy obrigatório: o escritório da dívida vem da entidade
        var query = db.Dividas
            .AsNoTracking()
            .Where(d => d.Entidade!.EscritorioId == escritorioId);

        if (filter.EntidadeId.HasValue)
            query = query.Where(d => d.EntidadeId == filter.EntidadeId.Value);

        if (filter.ResponsavelId.HasValue)
            query = query.Where(d => d.ResponsavelId == filter.ResponsavelId.Value);

        if (filter.TipoCobrancaId.HasValue)
            query = query.Where(d => d.TipoCobrancaId == filter.TipoCobrancaId.Value);

        if (!string.IsNullOrWhiteSpace(filter.NumeroCobranca))
            query = query.Where(d => d.NumeroCobranca.Contains(filter.NumeroCobranca));

        if (!string.IsNullOrWhiteSpace(filter.NomeAluno))
            query = query.Where(d => d.NomeAluno != null && d.NomeAluno.Contains(filter.NomeAluno));

        if (filter.VencimentoInicial.HasValue)
            query = query.Where(d => d.DataVencimento >= filter.VencimentoInicial.Value);

        if (filter.VencimentoFinal.HasValue)
            query = query.Where(d => d.DataVencimento <= filter.VencimentoFinal.Value);

        var total = await query.CountAsync();

        // Projeção enxuta: o grid só precisa de poucas colunas
        var items = await query
            .OrderBy(d => d.DataVencimento)
            .ThenBy(d => d.NumeroCobranca)
            .ThenBy(d => d.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DividaListItemDto
            {
                Id = d.Id,
                EntidadeNome = d.Entidade!.Nome,
                ResponsavelNome = d.Responsavel!.Nome,
                TipoCobrancaNome = d.TipoCobranca!.Nome,
                NumeroCobranca = d.NumeroCobranca,
                Parcela = d.Parcela,
                DataVencimento = d.DataVencimento,
                ValorCobranca = d.ValorCobranca,
                NomeAluno = d.NomeAluno
            })
            .ToListAsync();

        return new PagedResultDto<DividaListItemDto>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<DividaDto> GetByIdAsync(Guid id, Guid escritorioId)
    {
        var divida = await db.Dividas
            .AsNoTracking()
            .Include(d => d.Responsavel)
            .Include(d => d.ResponsavelAtual)
            .Include(d => d.AlteradoPor)
            .FirstOrDefaultAsync(d => d.Id == id && d.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Dívida {id} não encontrada.");

        return ToDto(divida);
    }

    public async Task<DividaDto> CreateAsync(DividaSaveDto dto, Guid escritorioId, int usuarioId)
    {
        await ValidateAsync(dto, escritorioId);

        var divida = new Divida { Id = Guid.NewGuid() };
        Apply(divida, dto);
        divida.AlteradoEm = DateTime.UtcNow;
        divida.AlteradoPorId = usuarioId;

        db.Dividas.Add(divida);
        await db.SaveChangesAsync();

        return await GetByIdAsync(divida.Id, escritorioId);
    }

    public async Task<DividaDto> UpdateAsync(Guid id, DividaSaveDto dto, Guid escritorioId, int usuarioId)
    {
        var divida = await db.Dividas
            .FirstOrDefaultAsync(d => d.Id == id && d.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Dívida {id} não encontrada.");

        // A entidade é definida na criação e não pode mais ser alterada
        if (dto.EntidadeId != divida.EntidadeId)
            throw new ArgumentException("A entidade da dívida não pode ser alterada.");

        await ValidateAsync(dto, escritorioId);

        Apply(divida, dto);
        divida.AlteradoEm = DateTime.UtcNow;
        divida.AlteradoPorId = usuarioId;

        await db.SaveChangesAsync();

        return await GetByIdAsync(id, escritorioId);
    }

    public async Task DeleteAsync(Guid id, Guid escritorioId)
    {
        var divida = await db.Dividas
            .FirstOrDefaultAsync(d => d.Id == id && d.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Dívida {id} não encontrada.");

        db.Dividas.Remove(divida);
        await db.SaveChangesAsync();
    }

    // ─── Validação ───────────────────────────────────────────

    private async Task ValidateAsync(DividaSaveDto dto, Guid escritorioId)
    {
        // Obrigatórios
        if (string.IsNullOrWhiteSpace(dto.NumeroCobranca))
            throw new ArgumentException("Número da cobrança é obrigatório.");
        if (dto.DataVencimento == default)
            throw new ArgumentException("Data de vencimento é obrigatória.");
        if (dto.TipoCobrancaId <= 0)
            throw new ArgumentException("Tipo de cobrança é obrigatório.");

        // Tamanhos de texto (evita erro de truncamento no banco)
        var textos = new (string Campo, string? Valor, int Max)[]
        {
            ("Número da cobrança", dto.NumeroCobranca, 20),
            ("Agência", dto.Agencia, 10),
            ("Conta", dto.Conta, 50),
            ("Parcela", dto.Parcela, 50),
            ("Número do contrato", dto.NumeroContrato, 50),
            ("Nome do aluno", dto.NomeAluno, 100),
            ("Série", dto.Serie, 50),
            ("Nome do responsável pela cobrança", dto.NomeResponsavelCobranca, 100),
            ("Pasta", dto.Pasta, 255),
            ("Número da vara", dto.NumeroVara, 255),
            ("Número do processo", dto.NumeroProcesso, 255),
        };
        foreach (var (campo, valor, max) in textos)
        {
            if (valor is not null && valor.Length > max)
                throw new ArgumentException($"{campo} deve ter no máximo {max} caracteres.");
        }

        // Limites numéricos (numeric(10,2), numeric(10,6), numeric(20,10))
        var valores = new (string Campo, decimal? Valor)[]
        {
            ("Valor da cobrança", dto.ValorCobranca), ("Valor da cobrança (ação)", dto.ValorCobrancaAcao),
            ("Valor da multa", dto.ValorMulta), ("Valor do juro", dto.ValorJuro), ("Valor pago", dto.ValorPago),
            ("Valor corrigido", dto.ValorCorrigido), ("Valor da multa JW", dto.ValorMultaJw),
            ("Valor do juro JW", dto.ValorJuroJw), ("Subtotal", dto.ValorSubtotal),
            ("Valor dos honorários", dto.ValorHonorarios), ("Valor total", dto.ValorTotal),
            ("Valor cobrado", dto.ValorCobrado),
        };
        foreach (var (campo, valor) in valores)
        {
            if (valor.HasValue && Math.Abs(valor.Value) >= 100_000_000m)
                throw new ArgumentException($"{campo} excede o limite permitido.");
        }

        var percentuais = new (string Campo, decimal? Valor)[]
        {
            ("Percentual de multa", dto.PercentualMulta), ("Percentual de juros", dto.PercentualJuros),
            ("Percentual de honorários", dto.PercentualHonorarios),
        };
        foreach (var (campo, valor) in percentuais)
        {
            if (valor.HasValue && Math.Abs(valor.Value) >= 10_000m)
                throw new ArgumentException($"{campo} excede o limite permitido.");
        }

        foreach (var (campo, valor) in new (string, decimal?)[] { ("Índice inicial", dto.IndiceInicial), ("Índice final", dto.IndiceFinal) })
        {
            if (valor.HasValue && Math.Abs(valor.Value) >= 10_000_000_000m)
                throw new ArgumentException($"{campo} excede o limite permitido.");
        }

        if (dto.MesInicioCorrecao is < 1 or > 12)
            throw new ArgumentException("Mês de início da correção deve estar entre 1 e 12.");
        if (dto.MesFimCorrecao is < 1 or > 12)
            throw new ArgumentException("Mês de fim da correção deve estar entre 1 e 12.");

        // Vínculos: tudo que pertence a escritório precisa ser do escritório do usuário
        Require(await db.Entidades.AnyAsync(e => e.Id == dto.EntidadeId && e.EscritorioId == escritorioId),
            "Entidade não encontrada para este escritório.");

        // Responsáveis precisam pertencer à entidade da dívida (que já é do escritório)
        Require(await ResponsavelDaEntidadeAsync(dto.ResponsavelId, dto.EntidadeId),
            "Responsável não encontrado para esta entidade.");

        if (dto.ResponsavelAtualId is { } atualId)
            Require(await ResponsavelDaEntidadeAsync(atualId, dto.EntidadeId),
                "Responsável atual não encontrado para esta entidade.");

        // Escola precisa pertencer à entidade da dívida (que já é do escritório)
        if (dto.EscolaId is { } escolaId)
            Require(await db.Escolas.AnyAsync(e => e.Id == escolaId && e.EntidadeId == dto.EntidadeId),
                "Escola não encontrada para esta entidade.");

        if (dto.PosicaoContratoId is { } posContratoId)
            Require(await db.PosicoesContrato.AnyAsync(p => p.Id == posContratoId && p.EscritorioId == escritorioId),
                "Posição do contrato não encontrada para este escritório.");

        if (dto.PosicaoChequeId is { } posChequeId)
            Require(await db.PosicoesCheque.AnyAsync(p => p.Id == posChequeId && p.EscritorioId == escritorioId),
                "Posição do cheque não encontrada para este escritório.");

        if (dto.AndamentoId is { } andamentoId)
            Require(await db.Andamentos.AnyAsync(a => a.Id == andamentoId && a.EscritorioId == escritorioId),
                "Andamento não encontrado para este escritório.");

        if (dto.LugarId is { } lugarId)
            Require(await db.Lugares.AnyAsync(l => l.Id == lugarId && l.EscritorioId == escritorioId),
                "Lugar não encontrado para este escritório.");

        // Tabelas de domínio globais
        Require(await db.TiposCobranca.AnyAsync(t => t.Id == dto.TipoCobrancaId),
            "Tipo de cobrança não encontrado.");

        if (dto.BancoId is { } bancoId)
            Require(await db.Bancos.AnyAsync(b => b.Id == bancoId), "Banco não encontrado.");

        if (dto.AlineaId is { } alineaId)
            Require(await db.Alineas.AnyAsync(a => a.Id == alineaId), "Alínea não encontrada.");
    }

    private Task<bool> ResponsavelDaEntidadeAsync(Guid responsavelId, Guid entidadeId)
        => db.Responsaveis.AnyAsync(r => r.Id == responsavelId && r.EntidadeId == entidadeId);

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new KeyNotFoundException(message);
    }

    // ─── Mapeamento ──────────────────────────────────────────

    /// <summary>Copia os campos editáveis. Campos somente leitura não são tocados.</summary>
    private static void Apply(Divida d, DividaSaveDto dto)
    {
        // Aba: Dívida
        d.EntidadeId = dto.EntidadeId;
        d.ResponsavelId = dto.ResponsavelId;
        d.TipoCobrancaId = dto.TipoCobrancaId;
        d.NumeroCobranca = dto.NumeroCobranca.Trim();
        d.BancoId = dto.BancoId;
        d.Agencia = dto.Agencia;
        d.Conta = dto.Conta;
        d.AlineaId = dto.AlineaId;
        d.DataVencimento = dto.DataVencimento;
        d.ValorCobranca = dto.ValorCobranca;
        d.ValorCobrancaAcao = dto.ValorCobrancaAcao;
        d.StatusValorAcao = dto.StatusValorAcao;
        d.Parcela = dto.Parcela;
        d.ValorMulta = dto.ValorMulta;
        d.ValorJuro = dto.ValorJuro;
        d.DataAcertoMantenedora = dto.DataAcertoMantenedora;
        d.ValorPago = dto.ValorPago;
        d.NumeroContrato = dto.NumeroContrato;
        d.ResponsavelAtualId = dto.ResponsavelAtualId;
        d.StatusBolsista = dto.StatusBolsista;
        d.StatusEstornado = dto.StatusEstornado;
        d.CodigoAluno = dto.CodigoAluno;
        d.NomeAluno = dto.NomeAluno;
        d.Serie = dto.Serie;
        d.Ano = dto.Ano;
        d.EscolaId = dto.EscolaId;

        // Aba: Cálculo
        d.DataInicioJuro = dto.DataInicioJuro;
        d.DataAcertoJw = dto.DataAcertoJw;
        d.NumeroDias = dto.NumeroDias;
        d.MesInicioCorrecao = dto.MesInicioCorrecao;
        d.AnoInicioCorrecao = dto.AnoInicioCorrecao;
        d.IndiceInicial = dto.IndiceInicial;
        d.MesFimCorrecao = dto.MesFimCorrecao;
        d.AnoFimCorrecao = dto.AnoFimCorrecao;
        d.IndiceFinal = dto.IndiceFinal;
        d.ValorCorrigido = dto.ValorCorrigido;
        d.PercentualMulta = dto.PercentualMulta;
        d.ValorMultaJw = dto.ValorMultaJw;
        d.PercentualJuros = dto.PercentualJuros;
        d.ValorJuroJw = dto.ValorJuroJw;
        d.ValorSubtotal = dto.ValorSubtotal;
        d.PercentualHonorarios = dto.PercentualHonorarios;
        d.ValorHonorarios = dto.ValorHonorarios;
        d.ValorTotal = dto.ValorTotal;
        d.StatusPagoJw = dto.StatusPagoJw;
        d.DataPagoJw = dto.DataPagoJw;
        d.ValorCobrado = dto.ValorCobrado;

        // Aba: Complemento
        d.NumeroRecibo = dto.NumeroRecibo;
        d.NomeResponsavelCobranca = dto.NomeResponsavelCobranca;
        d.PosicaoContratoId = dto.PosicaoContratoId;
        d.PosicaoChequeId = dto.PosicaoChequeId;
        d.AndamentoId = dto.AndamentoId;
        d.LugarId = dto.LugarId;
        d.StatusAcao = dto.StatusAcao;
        d.StatusSpcNaoEnviar = dto.StatusSpcNaoEnviar;
        d.StatusNaoEnviarCarta = dto.StatusNaoEnviarCarta;
        d.StatusAcordo = dto.StatusAcordo;
        d.StatusSpcBaixaManual = dto.StatusSpcBaixaManual;
        d.StatusSpcBaixar = dto.StatusSpcBaixar;
        d.Pasta = dto.Pasta;
        d.NumeroVara = dto.NumeroVara;
        d.NumeroProcesso = dto.NumeroProcesso;
        d.DataAcao = dto.DataAcao;
        d.DataSpcEnvio = dto.DataSpcEnvio;
        d.StatusSpcEnvio = dto.StatusSpcEnvio;
        d.DataSpcBaixa = dto.DataSpcBaixa;
        d.StatusSpcBaixa = dto.StatusSpcBaixa;
        d.RemessaSpcEnvio = dto.RemessaSpcEnvio;
        d.RemessaSpcBaixa = dto.RemessaSpcBaixa;
        d.DataAcertoJwMantenedora = dto.DataAcertoJwMantenedora;

        // Aba: Observações
        d.ObsJw = dto.ObsJw;
    }

    private static DividaDto ToDto(Divida d) => new()
    {
        Id = d.Id,

        // Aba: Dívida
        EntidadeId = d.EntidadeId,
        ResponsavelId = d.ResponsavelId,
        TipoCobrancaId = d.TipoCobrancaId,
        NumeroCobranca = d.NumeroCobranca,
        DataPrimeiraImportacao = d.DataPrimeiraImportacao,
        DataUltimaImportacao = d.DataUltimaImportacao,
        BancoId = d.BancoId,
        Agencia = d.Agencia,
        Conta = d.Conta,
        AlineaId = d.AlineaId,
        DataVencimento = d.DataVencimento,
        ValorCobranca = d.ValorCobranca,
        ValorCobrancaAcao = d.ValorCobrancaAcao,
        StatusValorAcao = d.StatusValorAcao,
        Parcela = d.Parcela,
        ValorMulta = d.ValorMulta,
        ValorJuro = d.ValorJuro,
        DataAcertoMantenedora = d.DataAcertoMantenedora,
        ValorPago = d.ValorPago,
        NumeroContrato = d.NumeroContrato,
        ResponsavelAtualId = d.ResponsavelAtualId,
        StatusBolsista = d.StatusBolsista,
        StatusEstornado = d.StatusEstornado,
        CodigoAluno = d.CodigoAluno,
        NomeAluno = d.NomeAluno,
        Serie = d.Serie,
        Ano = d.Ano,
        EscolaId = d.EscolaId,
        // datetime2 sai do banco sem Kind; marca como UTC para o cliente converter para o horário local
        AlteradoEm = DateTime.SpecifyKind(d.AlteradoEm, DateTimeKind.Utc),
        AlteradoPorId = d.AlteradoPorId,
        AlteradoPorNome = d.AlteradoPor?.Username,

        // Aba: Cálculo
        DataInicioJuro = d.DataInicioJuro,
        DataAcertoJw = d.DataAcertoJw,
        NumeroDias = d.NumeroDias,
        MesInicioCorrecao = d.MesInicioCorrecao,
        AnoInicioCorrecao = d.AnoInicioCorrecao,
        IndiceInicial = d.IndiceInicial,
        MesFimCorrecao = d.MesFimCorrecao,
        AnoFimCorrecao = d.AnoFimCorrecao,
        IndiceFinal = d.IndiceFinal,
        ValorCorrigido = d.ValorCorrigido,
        PercentualMulta = d.PercentualMulta,
        ValorMultaJw = d.ValorMultaJw,
        PercentualJuros = d.PercentualJuros,
        ValorJuroJw = d.ValorJuroJw,
        ValorSubtotal = d.ValorSubtotal,
        PercentualHonorarios = d.PercentualHonorarios,
        ValorHonorarios = d.ValorHonorarios,
        ValorTotal = d.ValorTotal,
        StatusPagoJw = d.StatusPagoJw,
        DataPagoJw = d.DataPagoJw,
        ValorCobrado = d.ValorCobrado,

        // Aba: Complemento
        NumeroRecibo = d.NumeroRecibo,
        DataImpressaoCarta = d.DataImpressaoCarta,
        NomeResponsavelCobranca = d.NomeResponsavelCobranca,
        PosicaoContratoId = d.PosicaoContratoId,
        PosicaoChequeId = d.PosicaoChequeId,
        AndamentoId = d.AndamentoId,
        LugarId = d.LugarId,
        StatusAcao = d.StatusAcao,
        StatusSpcNaoEnviar = d.StatusSpcNaoEnviar,
        StatusNaoEnviarCarta = d.StatusNaoEnviarCarta,
        StatusAcordo = d.StatusAcordo,
        StatusSpcBaixaManual = d.StatusSpcBaixaManual,
        StatusSpcBaixar = d.StatusSpcBaixar,
        Pasta = d.Pasta,
        NumeroVara = d.NumeroVara,
        NumeroProcesso = d.NumeroProcesso,
        DataAcao = d.DataAcao,
        DataSpcEnvio = d.DataSpcEnvio,
        StatusSpcEnvio = d.StatusSpcEnvio,
        DataSpcBaixa = d.DataSpcBaixa,
        StatusSpcBaixa = d.StatusSpcBaixa,
        RemessaSpcEnvio = d.RemessaSpcEnvio,
        RemessaSpcBaixa = d.RemessaSpcBaixa,
        DataAcertoJwMantenedora = d.DataAcertoJwMantenedora,

        // Aba: Observações
        ObsJw = d.ObsJw,

        // Rótulos dos autocompletes
        ResponsavelNome = d.Responsavel?.Nome,
        ResponsavelCpf = d.Responsavel?.Cpf,
        ResponsavelAtualNome = d.ResponsavelAtual?.Nome,
        ResponsavelAtualCpf = d.ResponsavelAtual?.Cpf
    };
}
