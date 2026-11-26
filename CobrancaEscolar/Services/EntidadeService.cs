using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Entidade;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class EntidadeService(AppDbContext db) : IEntidadeService
{
    public async Task<PagedResultDto<EntidadeDto>> GetAllAsync(EntidadeFilterDto filter, Guid escritorioId)
    {
        var query = db.Entidades
            .Where(e => e.EscritorioId == escritorioId) // filtro obrigatório de multitenancy
            .Include(e => e.Escritorio)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(e => e.Nome.Contains(filter.Nome));

        if (!string.IsNullOrWhiteSpace(filter.Codigo))
            query = query.Where(e => e.Codigo.Contains(filter.Codigo));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(e => e.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(e => ToDto(e))
            .ToListAsync();

        return new PagedResultDto<EntidadeDto>
        {
            Items = items,
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<EntidadeDto> GetByIdAsync(Guid id, Guid escritorioId)
    {
        var entidade = await db.Entidades
            .Include(e => e.Escritorio)
            .FirstOrDefaultAsync(e => e.Id == id && e.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Entidade {id} não encontrada.");

        return ToDto(entidade);
    }

    public async Task<EntidadeDto> CreateAsync(CreateEntidadeDto dto, Guid escritorioId)
    {
        if (await db.Entidades.AnyAsync(e => e.Codigo == dto.Codigo && e.EscritorioId == escritorioId))
            throw new InvalidOperationException($"Código '{dto.Codigo}' já existe neste escritório.");

        var entidade = new Entidade
        {
            Id = Guid.NewGuid(),
            Codigo = dto.Codigo,
            Nome = dto.Nome,
            EscritorioId = escritorioId
        };

        db.Entidades.Add(entidade);
        await db.SaveChangesAsync();
        return await GetByIdAsync(entidade.Id, escritorioId);
    }

    public async Task<EntidadeDto> UpdateAsync(Guid id, UpdateEntidadeDto dto, Guid escritorioId)
    {
        var entidade = await db.Entidades
            .FirstOrDefaultAsync(e => e.Id == id && e.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Entidade {id} não encontrada.");

        if (await db.Entidades.AnyAsync(e => e.Codigo == dto.Codigo && e.EscritorioId == escritorioId && e.Id != id))
            throw new InvalidOperationException($"Código '{dto.Codigo}' já existe neste escritório.");

        entidade.Codigo = dto.Codigo;
        entidade.Nome = dto.Nome;

        await db.SaveChangesAsync();
        return await GetByIdAsync(id, escritorioId);
    }

    public async Task DeleteAsync(Guid id, Guid escritorioId)
    {
        var entidade = await db.Entidades
            .FirstOrDefaultAsync(e => e.Id == id && e.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Entidade {id} não encontrada.");

        var temEscolas = await db.Escolas.AnyAsync(e => e.EntidadeId == id);
        if (temEscolas)
            throw new InvalidOperationException("Não é possível excluir: entidade possui escolas vinculadas.");

        var temResponsaveis = await db.Responsaveis.AnyAsync(r => r.EntidadeId == id);
        if (temResponsaveis)
            throw new InvalidOperationException("Não é possível excluir: entidade possui responsáveis vinculados.");

        db.Entidades.Remove(entidade);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<EntidadeDto>> GetComboAsync(Guid escritorioId)
    {
        return await db.Entidades
            .Where(e => e.EscritorioId == escritorioId)
            .OrderBy(e => e.Nome)
            .Select(e => ToDto(e))
            .ToListAsync();
    }

    private static EntidadeDto ToDto(Entidade e) => new()
    {
        Id = e.Id,
        Codigo = e.Codigo,
        Nome = e.Nome,
        EscritorioId = e.EscritorioId,
        EscritorioNome = e.Escritorio?.Nome
    };
}
