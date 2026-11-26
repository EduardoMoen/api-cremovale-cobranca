using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Indice;
using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class IndiceService(AppDbContext db)
{
    public async Task<PagedResultDto<IndiceDto>> GetAllAsync(IndiceFilterDto filter)
    {
        var query = db.Indices.AsQueryable();

        if (filter.Ano.HasValue)
            query = query.Where(i => i.Ano == filter.Ano.Value);

        if (filter.Mes.HasValue)
            query = query.Where(i => i.Mes == filter.Mes.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(i => i.Ano)
            .ThenByDescending(i => i.Mes)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(i => ToDto(i))
            .ToListAsync();

        return new PagedResultDto<IndiceDto> { Items = items, TotalCount = total, Page = filter.Page, PageSize = filter.PageSize };
    }

    public async Task<IndiceDto> GetByIdAsync(int id)
    {
        var i = await db.Indices.FindAsync(id) ?? throw new KeyNotFoundException($"Índice {id} não encontrado.");
        return ToDto(i);
    }

    public async Task<IndiceDto> CreateAsync(CreateIndiceDto dto)
    {
        if (dto.Mes < 1 || dto.Mes > 12)
            throw new ArgumentException("Mês inválido.");

        if (await db.Indices.AnyAsync(i => i.Ano == dto.Ano && i.Mes == dto.Mes))
            throw new InvalidOperationException($"Índice para {dto.Mes:00}/{dto.Ano} já existe.");

        var i = new Indice { Ano = dto.Ano, Mes = dto.Mes, ValorIndice = dto.ValorIndice };
        db.Indices.Add(i);
        await db.SaveChangesAsync();
        return ToDto(i);
    }

    public async Task<IndiceDto> UpdateAsync(int id, UpdateIndiceDto dto)
    {
        var i = await db.Indices.FindAsync(id) ?? throw new KeyNotFoundException($"Índice {id} não encontrado.");
        i.ValorIndice = dto.ValorIndice;
        await db.SaveChangesAsync();
        return ToDto(i);
    }

    public async Task DeleteAsync(int id)
    {
        var i = await db.Indices.FindAsync(id) ?? throw new KeyNotFoundException($"Índice {id} não encontrado.");
        db.Indices.Remove(i);
        await db.SaveChangesAsync();
    }

    private static IndiceDto ToDto(Indice i) => new() { Id = i.Id, Ano = i.Ano, Mes = i.Mes, ValorIndice = i.ValorIndice };
}
