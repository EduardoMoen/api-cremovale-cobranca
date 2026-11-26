using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.PosicaoCheque;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class PosicaoChequeService(AppDbContext db)
    : IEscritorioScopedService<PosicaoChequeDto, CreatePosicaoChequeDto, UpdatePosicaoChequeDto, PosicaoChequeFilterDto>
{
    public async Task<PagedResultDto<PosicaoChequeDto>> GetAllAsync(PosicaoChequeFilterDto filter, Guid escritorioId)
    {
        var query = db.PosicoesCheque.Where(p => p.EscritorioId == escritorioId);

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(p => p.Nome.Contains(filter.Nome));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => ToDto(p))
            .ToListAsync();

        return new PagedResultDto<PosicaoChequeDto> { Items = items, TotalCount = total, Page = filter.Page, PageSize = filter.PageSize };
    }

    public async Task<PosicaoChequeDto> GetByIdAsync(Guid id, Guid escritorioId)
    {
        var p = await db.PosicoesCheque.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"PosicaoCheque {id} não encontrada.");
        return ToDto(p);
    }

    public async Task<PosicaoChequeDto> CreateAsync(CreatePosicaoChequeDto dto, Guid escritorioId)
    {
        if (await db.PosicoesCheque.AnyAsync(p => p.Nome == dto.Nome && p.EscritorioId == escritorioId))
            throw new InvalidOperationException($"Posição de cheque '{dto.Nome}' já existe.");

        var p = new PosicaoCheque { Id = Guid.NewGuid(), Nome = dto.Nome, EscritorioId = escritorioId };
        db.PosicoesCheque.Add(p);
        await db.SaveChangesAsync();
        return ToDto(p);
    }

    public async Task<PosicaoChequeDto> UpdateAsync(Guid id, UpdatePosicaoChequeDto dto, Guid escritorioId)
    {
        var p = await db.PosicoesCheque.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"PosicaoCheque {id} não encontrada.");

        if (await db.PosicoesCheque.AnyAsync(x => x.Nome == dto.Nome && x.EscritorioId == escritorioId && x.Id != id))
            throw new InvalidOperationException($"Posição de cheque '{dto.Nome}' já existe.");

        p.Nome = dto.Nome;
        await db.SaveChangesAsync();
        return ToDto(p);
    }

    public async Task DeleteAsync(Guid id, Guid escritorioId)
    {
        var p = await db.PosicoesCheque.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"PosicaoCheque {id} não encontrada.");
        db.PosicoesCheque.Remove(p);
        await db.SaveChangesAsync();
    }

    private static PosicaoChequeDto ToDto(PosicaoCheque p) => new() { Id = p.Id, Nome = p.Nome, EscritorioId = p.EscritorioId };
}
