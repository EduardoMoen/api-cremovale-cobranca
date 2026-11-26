using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Alinea;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class AlineaService(AppDbContext db)
{
    public async Task<PagedResultDto<AlineaDto>> GetAllAsync(AlineaFilterDto filter)
    {
        var query = db.Alineas.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(a => a.Nome.Contains(filter.Nome));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(a => a.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(a => ToDto(a))
            .ToListAsync();

        return new PagedResultDto<AlineaDto> { Items = items, TotalCount = total, Page = filter.Page, PageSize = filter.PageSize };
    }

    public async Task<AlineaDto> GetByIdAsync(int id)
    {
        var a = await db.Alineas.FindAsync(id) ?? throw new KeyNotFoundException($"Alínea {id} não encontrada.");
        return ToDto(a);
    }

    public async Task<AlineaDto> CreateAsync(CreateAlineaDto dto)
    {
        if (await db.Alineas.AnyAsync(a => a.Nome == dto.Nome))
            throw new InvalidOperationException($"Alínea '{dto.Nome}' já existe.");

        var a = new Alinea { Nome = dto.Nome, DataUltimaAlteracao = DateTime.UtcNow };
        db.Alineas.Add(a);
        await db.SaveChangesAsync();
        return ToDto(a);
    }

    public async Task<AlineaDto> UpdateAsync(int id, UpdateAlineaDto dto)
    {
        var a = await db.Alineas.FindAsync(id) ?? throw new KeyNotFoundException($"Alínea {id} não encontrada.");
        if (await db.Alineas.AnyAsync(x => x.Nome == dto.Nome && x.Id != id))
            throw new InvalidOperationException($"Alínea '{dto.Nome}' já existe.");

        a.Nome = dto.Nome;
        a.DataUltimaAlteracao = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return ToDto(a);
    }

    public async Task DeleteAsync(int id)
    {
        var a = await db.Alineas.FindAsync(id) ?? throw new KeyNotFoundException($"Alínea {id} não encontrada.");
        db.Alineas.Remove(a);
        await db.SaveChangesAsync();
    }

    private static AlineaDto ToDto(Alinea a) => new() { Id = a.Id, Nome = a.Nome, DataUltimaAlteracao = a.DataUltimaAlteracao };
}
