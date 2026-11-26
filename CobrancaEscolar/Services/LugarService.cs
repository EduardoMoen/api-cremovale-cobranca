using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Lugar;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class LugarService(AppDbContext db)
    : IEscritorioScopedService<LugarDto, CreateLugarDto, UpdateLugarDto, LugarFilterDto>
{
    public async Task<PagedResultDto<LugarDto>> GetAllAsync(LugarFilterDto filter, Guid escritorioId)
    {
        var query = db.Lugares.Where(l => l.EscritorioId == escritorioId);

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(l => l.Nome.Contains(filter.Nome));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(l => l.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(l => ToDto(l))
            .ToListAsync();

        return new PagedResultDto<LugarDto> { Items = items, TotalCount = total, Page = filter.Page, PageSize = filter.PageSize };
    }

    public async Task<LugarDto> GetByIdAsync(Guid id, Guid escritorioId)
    {
        var l = await db.Lugares.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Lugar {id} não encontrado.");
        return ToDto(l);
    }

    public async Task<LugarDto> CreateAsync(CreateLugarDto dto, Guid escritorioId)
    {
        if (await db.Lugares.AnyAsync(l => l.Nome == dto.Nome && l.EscritorioId == escritorioId))
            throw new InvalidOperationException($"Lugar '{dto.Nome}' já existe.");

        var l = new Lugar { Id = Guid.NewGuid(), Nome = dto.Nome, EscritorioId = escritorioId };
        db.Lugares.Add(l);
        await db.SaveChangesAsync();
        return ToDto(l);
    }

    public async Task<LugarDto> UpdateAsync(Guid id, UpdateLugarDto dto, Guid escritorioId)
    {
        var l = await db.Lugares.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Lugar {id} não encontrado.");

        if (await db.Lugares.AnyAsync(x => x.Nome == dto.Nome && x.EscritorioId == escritorioId && x.Id != id))
            throw new InvalidOperationException($"Lugar '{dto.Nome}' já existe.");

        l.Nome = dto.Nome;
        await db.SaveChangesAsync();
        return ToDto(l);
    }

    public async Task DeleteAsync(Guid id, Guid escritorioId)
    {
        var l = await db.Lugares.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Lugar {id} não encontrado.");
        db.Lugares.Remove(l);
        await db.SaveChangesAsync();
    }

    private static LugarDto ToDto(Lugar l) => new() { Id = l.Id, Nome = l.Nome, EscritorioId = l.EscritorioId };
}
