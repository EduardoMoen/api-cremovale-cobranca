using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.PosicaoContrato;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class PosicaoContratoService(AppDbContext db)
    : IEscritorioScopedService<PosicaoContratoDto, CreatePosicaoContratoDto, UpdatePosicaoContratoDto, PosicaoContratoFilterDto>
{
    public async Task<PagedResultDto<PosicaoContratoDto>> GetAllAsync(PosicaoContratoFilterDto filter, Guid escritorioId)
    {
        var query = db.PosicoesContrato.Where(p => p.EscritorioId == escritorioId);

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(p => p.Nome.Contains(filter.Nome));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => ToDto(p))
            .ToListAsync();

        return new PagedResultDto<PosicaoContratoDto> { Items = items, TotalCount = total, Page = filter.Page, PageSize = filter.PageSize };
    }

    public async Task<PosicaoContratoDto> GetByIdAsync(Guid id, Guid escritorioId)
    {
        var p = await db.PosicoesContrato.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"PosicaoContrato {id} não encontrada.");
        return ToDto(p);
    }

    public async Task<PosicaoContratoDto> CreateAsync(CreatePosicaoContratoDto dto, Guid escritorioId)
    {
        if (await db.PosicoesContrato.AnyAsync(p => p.Nome == dto.Nome && p.EscritorioId == escritorioId))
            throw new InvalidOperationException($"Posição de contrato '{dto.Nome}' já existe.");

        var p = new PosicaoContrato { Id = Guid.NewGuid(), Nome = dto.Nome, EscritorioId = escritorioId };
        db.PosicoesContrato.Add(p);
        await db.SaveChangesAsync();
        return ToDto(p);
    }

    public async Task<PosicaoContratoDto> UpdateAsync(Guid id, UpdatePosicaoContratoDto dto, Guid escritorioId)
    {
        var p = await db.PosicoesContrato.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"PosicaoContrato {id} não encontrada.");

        if (await db.PosicoesContrato.AnyAsync(x => x.Nome == dto.Nome && x.EscritorioId == escritorioId && x.Id != id))
            throw new InvalidOperationException($"Posição de contrato '{dto.Nome}' já existe.");

        p.Nome = dto.Nome;
        await db.SaveChangesAsync();
        return ToDto(p);
    }

    public async Task DeleteAsync(Guid id, Guid escritorioId)
    {
        var p = await db.PosicoesContrato.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"PosicaoContrato {id} não encontrada.");
        db.PosicoesContrato.Remove(p);
        await db.SaveChangesAsync();
    }

    private static PosicaoContratoDto ToDto(PosicaoContrato p) => new() { Id = p.Id, Nome = p.Nome, EscritorioId = p.EscritorioId };
}
