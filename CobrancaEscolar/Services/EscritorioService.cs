using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Escritorio;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class EscritorioService(AppDbContext db) : IEscritorioService
{
    public async Task<PagedResultDto<EscritorioDto>> GetAllAsync(EscritorioFilterDto filter)
    {
        var query = db.Escritorios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(e => e.Nome.Contains(filter.Nome));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(e => e.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(e => ToDto(e))
            .ToListAsync();

        return new PagedResultDto<EscritorioDto>
        {
            Items = items,
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<EscritorioDto> GetByIdAsync(Guid id)
    {
        var escritorio = await db.Escritorios.FindAsync(id)
            ?? throw new KeyNotFoundException($"Escritório {id} não encontrado.");

        return ToDto(escritorio);
    }

    public async Task<EscritorioDto> CreateAsync(CreateEscritorioDto dto)
    {
        if (await db.Escritorios.AnyAsync(e => e.Nome == dto.Nome))
            throw new InvalidOperationException($"Escritório '{dto.Nome}' já existe.");

        var escritorio = new Escritorio
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Email = dto.Email,
            Endereco = dto.Endereco,
            Fone = dto.Fone
        };

        db.Escritorios.Add(escritorio);
        await db.SaveChangesAsync();
        return ToDto(escritorio);
    }

    public async Task<EscritorioDto> UpdateAsync(Guid id, UpdateEscritorioDto dto)
    {
        var escritorio = await db.Escritorios.FindAsync(id)
            ?? throw new KeyNotFoundException($"Escritório {id} não encontrado.");

        if (await db.Escritorios.AnyAsync(e => e.Nome == dto.Nome && e.Id != id))
            throw new InvalidOperationException($"Escritório '{dto.Nome}' já existe.");

        escritorio.Nome = dto.Nome;
        escritorio.Email = dto.Email;
        escritorio.Endereco = dto.Endereco;
        escritorio.Fone = dto.Fone;

        await db.SaveChangesAsync();
        return ToDto(escritorio);
    }

    public async Task DeleteAsync(Guid id)
    {
        var escritorio = await db.Escritorios.FindAsync(id)
            ?? throw new KeyNotFoundException($"Escritório {id} não encontrado.");

        var temUsuarios = await db.Usuarios.AnyAsync(u => u.EscritorioId == id);
        if (temUsuarios)
            throw new InvalidOperationException("Não é possível excluir: escritório possui usuários vinculados.");

        var temEntidades = await db.Entidades.AnyAsync(e => e.EscritorioId == id);
        if (temEntidades)
            throw new InvalidOperationException("Não é possível excluir: escritório possui entidades vinculadas.");

        db.Escritorios.Remove(escritorio);
        await db.SaveChangesAsync();
    }

    private static EscritorioDto ToDto(Escritorio e) => new()
    {
        Id = e.Id,
        Nome = e.Nome,
        Email = e.Email,
        Endereco = e.Endereco,
        Fone = e.Fone
    };
}
