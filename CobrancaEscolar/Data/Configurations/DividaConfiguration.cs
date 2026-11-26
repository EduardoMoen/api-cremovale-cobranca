using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CobrancaEscolar.Data.Configurations;

public class DividaConfiguration : IEntityTypeConfiguration<Divida>
{
    public void Configure(EntityTypeBuilder<Divida> builder)
    {
        builder.ToTable("divida");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id").HasDefaultValueSql("NEWSEQUENTIALID()");

        // ─── Aba: Dívida ────────────────────────────────────
        builder.Property(d => d.EntidadeId).HasColumnName("entidade_id").IsRequired();
        builder.Property(d => d.ResponsavelId).HasColumnName("responsavel_id").IsRequired();
        builder.Property(d => d.TipoCobrancaId).HasColumnName("tipoCobranca_id").IsRequired();
        builder.Property(d => d.NumeroCobranca).HasColumnName("numeroCobranca").HasMaxLength(20).IsRequired();
        builder.Property(d => d.DataPrimeiraImportacao).HasColumnName("dataPrimeiraImportacao");
        builder.Property(d => d.DataUltimaImportacao).HasColumnName("dataUltimaImportacao");
        builder.Property(d => d.BancoId).HasColumnName("banco_id");
        builder.Property(d => d.Agencia).HasColumnName("agencia").HasMaxLength(10);
        builder.Property(d => d.Conta).HasColumnName("conta").HasMaxLength(50);
        builder.Property(d => d.AlineaId).HasColumnName("alinea_id");
        builder.Property(d => d.DataVencimento).HasColumnName("dataVencimento").IsRequired();
        builder.Property(d => d.ValorCobranca).HasColumnName("valorCobranca").HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(d => d.ValorCobrancaAcao).HasColumnName("valorCobrancaAcao").HasColumnType("numeric(10,2)");
        builder.Property(d => d.StatusValorAcao).HasColumnName("statusValorAcao");
        builder.Property(d => d.Parcela).HasColumnName("parcela").HasMaxLength(50);
        builder.Property(d => d.ValorMulta).HasColumnName("valorMulta").HasColumnType("numeric(10,2)");
        builder.Property(d => d.ValorJuro).HasColumnName("valorJuro").HasColumnType("numeric(10,2)");
        builder.Property(d => d.DataAcertoMantenedora).HasColumnName("dataAcertoMantenedora");
        builder.Property(d => d.ValorPago).HasColumnName("valorPago").HasColumnType("numeric(10,2)");
        builder.Property(d => d.NumeroContrato).HasColumnName("numeroContrato").HasMaxLength(50);
        builder.Property(d => d.ResponsavelAtualId).HasColumnName("responsavelAtual_id");
        builder.Property(d => d.StatusBolsista).HasColumnName("statusBolsista");
        builder.Property(d => d.StatusEstornado).HasColumnName("statusEstornado");
        builder.Property(d => d.CodigoAluno).HasColumnName("codigoAluno");
        builder.Property(d => d.NomeAluno).HasColumnName("nomeAluno").HasMaxLength(100);
        builder.Property(d => d.Serie).HasColumnName("serie").HasMaxLength(50);
        builder.Property(d => d.Ano).HasColumnName("ano");
        builder.Property(d => d.EscolaId).HasColumnName("escola_id");
        builder.Property(d => d.AlteradoEm).HasColumnName("alterado_em").IsRequired();
        builder.Property(d => d.AlteradoPorId).HasColumnName("alterado_por_id");

        // ─── Aba: Cálculo ───────────────────────────────────
        builder.Property(d => d.DataInicioJuro).HasColumnName("dataInicioJuro");
        builder.Property(d => d.DataAcertoJw).HasColumnName("dataAcertoJw");
        builder.Property(d => d.NumeroDias).HasColumnName("numeroDias");
        builder.Property(d => d.MesInicioCorrecao).HasColumnName("mesInicioCorrecao");
        builder.Property(d => d.AnoInicioCorrecao).HasColumnName("anoInicioCorrecao");
        builder.Property(d => d.IndiceInicial).HasColumnName("indiceInicial").HasColumnType("numeric(20,10)");
        builder.Property(d => d.MesFimCorrecao).HasColumnName("mesFimCorrecao");
        builder.Property(d => d.AnoFimCorrecao).HasColumnName("anoFimCorrecao");
        builder.Property(d => d.IndiceFinal).HasColumnName("indiceFinal").HasColumnType("numeric(20,10)");
        builder.Property(d => d.ValorCorrigido).HasColumnName("valorCorrigido").HasColumnType("numeric(10,2)");
        builder.Property(d => d.PercentualMulta).HasColumnName("percentualMulta").HasColumnType("numeric(10,6)");
        builder.Property(d => d.ValorMultaJw).HasColumnName("valorMultaJw").HasColumnType("numeric(10,2)");
        builder.Property(d => d.PercentualJuros).HasColumnName("percentualJuros").HasColumnType("numeric(10,6)");
        builder.Property(d => d.ValorJuroJw).HasColumnName("valorJuroJw").HasColumnType("numeric(10,2)");
        builder.Property(d => d.ValorSubtotal).HasColumnName("valorSubtotal").HasColumnType("numeric(10,2)");
        builder.Property(d => d.PercentualHonorarios).HasColumnName("percentualHonorarios").HasColumnType("numeric(10,6)");
        builder.Property(d => d.ValorHonorarios).HasColumnName("valorHonorarios").HasColumnType("numeric(10,2)");
        builder.Property(d => d.ValorTotal).HasColumnName("valorTotal").HasColumnType("numeric(10,2)");
        builder.Property(d => d.StatusPagoJw).HasColumnName("statusPagoJw");
        builder.Property(d => d.DataPagoJw).HasColumnName("dataPagoJw");
        builder.Property(d => d.ValorCobrado).HasColumnName("valorCobrado").HasColumnType("numeric(10,2)");

        // ─── Aba: Complemento ───────────────────────────────
        builder.Property(d => d.NumeroRecibo).HasColumnName("numeroRecibo");
        builder.Property(d => d.DataImpressaoCarta).HasColumnName("dataImpressaoCarta");
        builder.Property(d => d.NomeResponsavelCobranca).HasColumnName("nomeResponsavelCobranca").HasMaxLength(100);
        builder.Property(d => d.PosicaoContratoId).HasColumnName("posicaoContrato_id");
        builder.Property(d => d.PosicaoChequeId).HasColumnName("posicaoCheque_id");
        builder.Property(d => d.AndamentoId).HasColumnName("andamento_id");
        builder.Property(d => d.LugarId).HasColumnName("lugar_id");
        builder.Property(d => d.StatusAcao).HasColumnName("statusAcao");
        builder.Property(d => d.StatusSpcNaoEnviar).HasColumnName("statusSpcNaoEnviar");
        builder.Property(d => d.StatusNaoEnviarCarta).HasColumnName("statusNaoEnviarCarta");
        builder.Property(d => d.StatusAcordo).HasColumnName("statusAcordo");
        builder.Property(d => d.StatusSpcBaixaManual).HasColumnName("statusSpcBaixaManual");
        builder.Property(d => d.StatusSpcBaixar).HasColumnName("statusSpcBaixar");
        builder.Property(d => d.Pasta).HasColumnName("pasta").HasMaxLength(255);
        builder.Property(d => d.NumeroVara).HasColumnName("numeroVara").HasMaxLength(255);
        builder.Property(d => d.NumeroProcesso).HasColumnName("numeroProcesso").HasMaxLength(255);
        builder.Property(d => d.DataAcao).HasColumnName("dataAcao");
        builder.Property(d => d.DataSpcEnvio).HasColumnName("dataSpcEnvio");
        builder.Property(d => d.StatusSpcEnvio).HasColumnName("statusSpcEnvio");
        builder.Property(d => d.DataSpcBaixa).HasColumnName("dataSpcBaixa");
        builder.Property(d => d.StatusSpcBaixa).HasColumnName("statusSpcBaixa");
        builder.Property(d => d.RemessaSpcEnvio).HasColumnName("remessaSpcEnvio");
        builder.Property(d => d.RemessaSpcBaixa).HasColumnName("remessaSpcBaixa");
        builder.Property(d => d.DataAcertoJwMantenedora).HasColumnName("dataAcertoJwMantenedora");

        // ─── Aba: Observações ───────────────────────────────
        builder.Property(d => d.ObsJw).HasColumnName("obsJw");

        // ─── Índices ────────────────────────────────────────
        builder.HasIndex(d => d.EntidadeId).HasDatabaseName("ix_divida_entidade_id");
        builder.HasIndex(d => d.ResponsavelId).HasDatabaseName("ix_divida_responsavel_id");
        builder.HasIndex(d => d.NumeroCobranca).HasDatabaseName("ix_divida_numeroCobranca");
        builder.HasIndex(d => d.DataVencimento).HasDatabaseName("ix_divida_dataVencimento");

        // ─── Relacionamentos (sem cascade) ──────────────────
        builder.HasOne(d => d.Entidade).WithMany().HasForeignKey(d => d.EntidadeId)
            .HasConstraintName("fk_divida_entidade").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.Responsavel).WithMany().HasForeignKey(d => d.ResponsavelId)
            .HasConstraintName("fk_divida_responsavel").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.ResponsavelAtual).WithMany().HasForeignKey(d => d.ResponsavelAtualId)
            .HasConstraintName("fk_divida_responsavel_atual").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.TipoCobranca).WithMany().HasForeignKey(d => d.TipoCobrancaId)
            .HasConstraintName("fk_divida_tipocobranca").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.Banco).WithMany().HasForeignKey(d => d.BancoId)
            .HasConstraintName("fk_divida_banco").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.Alinea).WithMany().HasForeignKey(d => d.AlineaId)
            .HasConstraintName("fk_divida_alinea").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.Escola).WithMany().HasForeignKey(d => d.EscolaId)
            .HasConstraintName("fk_divida_escola").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.AlteradoPor).WithMany().HasForeignKey(d => d.AlteradoPorId)
            .HasConstraintName("fk_divida_alterado_por").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.PosicaoContrato).WithMany().HasForeignKey(d => d.PosicaoContratoId)
            .HasConstraintName("fk_divida_posicaocontrato").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.PosicaoCheque).WithMany().HasForeignKey(d => d.PosicaoChequeId)
            .HasConstraintName("fk_divida_posicaocheque").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.Andamento).WithMany().HasForeignKey(d => d.AndamentoId)
            .HasConstraintName("fk_divida_andamento").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.Lugar).WithMany().HasForeignKey(d => d.LugarId)
            .HasConstraintName("fk_divida_lugar").OnDelete(DeleteBehavior.NoAction);
    }
}
