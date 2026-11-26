using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.TipoCobranca;
using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class TipoCobrancaService(AppDbContext db)
{
    public async Task<PagedResultDto<TipoCobrancaDto>> GetAllAsync(TipoCobrancaFilterDto filter)
    {
        var query = db.TiposCobranca.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(t => t.Nome.Contains(filter.Nome));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(t => t.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(t => ToDto(t))
            .ToListAsync();

        return new PagedResultDto<TipoCobrancaDto> { Items = items, TotalCount = total, Page = filter.Page, PageSize = filter.PageSize };
    }

    public async Task<TipoCobrancaDto> GetByIdAsync(int id)
    {
        var t = await db.TiposCobranca.FindAsync(id) ?? throw new KeyNotFoundException($"TipoCobrança {id} não encontrado.");
        return ToDto(t);
    }

    public async Task<TipoCobrancaDto> CreateAsync(CreateTipoCobrancaDto dto)
    {
        if (await db.TiposCobranca.AnyAsync(t => t.Nome == dto.Nome))
            throw new InvalidOperationException($"TipoCobrança '{dto.Nome}' já existe.");

        var t = new TipoCobranca { Nome = dto.Nome };
        db.TiposCobranca.Add(t);
        await db.SaveChangesAsync();
        return ToDto(t);
    }

    public async Task<TipoCobrancaDto> UpdateAsync(int id, UpdateTipoCobrancaDto dto)
    {
        var t = await db.TiposCobranca.FindAsync(id) ?? throw new KeyNotFoundException($"TipoCobrança {id} não encontrado.");
        if (await db.TiposCobranca.AnyAsync(x => x.Nome == dto.Nome && x.Id != id))
            throw new InvalidOperationException($"TipoCobrança '{dto.Nome}' já existe.");

        t.Nome = dto.Nome;
        await db.SaveChangesAsync();
        return ToDto(t);
    }

    public async Task DeleteAsync(int id)
    {
        var t = await db.TiposCobranca.FindAsync(id) ?? throw new KeyNotFoundException($"TipoCobrança {id} não encontrado.");
        db.TiposCobranca.Remove(t);
        await db.SaveChangesAsync();
    }

    private static TipoCobrancaDto ToDto(TipoCobranca t) => new() { Id = t.Id, Nome = t.Nome };
}
