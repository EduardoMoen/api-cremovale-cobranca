using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Andamento;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class AndamentoService(AppDbContext db)
    : IEscritorioScopedService<AndamentoDto, CreateAndamentoDto, UpdateAndamentoDto, AndamentoFilterDto>
{
    public async Task<PagedResultDto<AndamentoDto>> GetAllAsync(AndamentoFilterDto filter, Guid escritorioId)
    {
        var query = db.Andamentos.Where(a => a.EscritorioId == escritorioId);

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(a => a.Nome.Contains(filter.Nome));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(a => a.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(a => ToDto(a))
            .ToListAsync();

        return new PagedResultDto<AndamentoDto> { Items = items, TotalCount = total, Page = filter.Page, PageSize = filter.PageSize };
    }

    public async Task<AndamentoDto> GetByIdAsync(Guid id, Guid escritorioId)
    {
        var a = await db.Andamentos.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Andamento {id} não encontrado.");
        return ToDto(a);
    }

    public async Task<AndamentoDto> CreateAsync(CreateAndamentoDto dto, Guid escritorioId)
    {
        if (await db.Andamentos.AnyAsync(a => a.Nome == dto.Nome && a.EscritorioId == escritorioId))
            throw new InvalidOperationException($"Andamento '{dto.Nome}' já existe.");

        var a = new Andamento { Id = Guid.NewGuid(), Nome = dto.Nome, EscritorioId = escritorioId };
        db.Andamentos.Add(a);
        await db.SaveChangesAsync();
        return ToDto(a);
    }

    public async Task<AndamentoDto> UpdateAsync(Guid id, UpdateAndamentoDto dto, Guid escritorioId)
    {
        var a = await db.Andamentos.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Andamento {id} não encontrado.");

        if (await db.Andamentos.AnyAsync(x => x.Nome == dto.Nome && x.EscritorioId == escritorioId && x.Id != id))
            throw new InvalidOperationException($"Andamento '{dto.Nome}' já existe.");

        a.Nome = dto.Nome;
        await db.SaveChangesAsync();
        return ToDto(a);
    }

    public async Task DeleteAsync(Guid id, Guid escritorioId)
    {
        var a = await db.Andamentos.FirstOrDefaultAsync(x => x.Id == id && x.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Andamento {id} não encontrado.");
        db.Andamentos.Remove(a);
        await db.SaveChangesAsync();
    }

    private static AndamentoDto ToDto(Andamento a) => new() { Id = a.Id, Nome = a.Nome, EscritorioId = a.EscritorioId };
}
