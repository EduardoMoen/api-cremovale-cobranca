using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Escola;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class EscolaService(AppDbContext db) : IEscolaService
{
    public async Task<PagedResultDto<EscolaDto>> GetAllAsync(EscolaFilterDto filter, Guid escritorioId)
    {
        // Multitenancy: busca somente escolas cujas entidades pertencem ao escritório do usuário
        var query = db.Escolas
            .Include(e => e.Entidade)
            .Where(e => e.Entidade!.EscritorioId == escritorioId);

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

        return new PagedResultDto<EscolaDto>
        {
            Items = items,
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<EscolaDto> GetByIdAsync(Guid id, Guid escritorioId)
    {
        var escola = await db.Escolas
            .Include(e => e.Entidade)
            .FirstOrDefaultAsync(e => e.Id == id && e.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Escola {id} não encontrada.");

        return ToDto(escola);
    }

    public async Task<EscolaDto> CreateAsync(CreateEscolaDto dto, Guid escritorioId)
    {
        // Garante que a entidade informada pertence ao escritório do usuário
        var entidade = await db.Entidades
            .FirstOrDefaultAsync(e => e.Id == dto.EntidadeId && e.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException("Entidade não encontrada para este escritório.");

        if (await db.Escolas.AnyAsync(e => e.Codigo == dto.Codigo && e.EntidadeId == dto.EntidadeId))
            throw new InvalidOperationException($"Código '{dto.Codigo}' já existe nesta entidade.");

        var escola = new Escola
        {
            Id = Guid.NewGuid(),
            Codigo = dto.Codigo,
            Nome = dto.Nome,
            Endereco = dto.Endereco,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Uf = dto.Uf,
            Cep = dto.Cep,
            Email = dto.Email,
            Obs = dto.Obs,
            EntidadeId = dto.EntidadeId
        };

        db.Escolas.Add(escola);
        await db.SaveChangesAsync();
        return await GetByIdAsync(escola.Id, escritorioId);
    }

    public async Task<EscolaDto> UpdateAsync(Guid id, UpdateEscolaDto dto, Guid escritorioId)
    {
        var escola = await db.Escolas
            .Include(e => e.Entidade)
            .FirstOrDefaultAsync(e => e.Id == id && e.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Escola {id} não encontrada.");

        // Valida que a nova entidade (se alterada) pertence ao escritório
        var entidade = await db.Entidades
            .FirstOrDefaultAsync(e => e.Id == dto.EntidadeId && e.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException("Entidade não encontrada para este escritório.");

        if (await db.Escolas.AnyAsync(e => e.Codigo == dto.Codigo && e.EntidadeId == dto.EntidadeId && e.Id != id))
            throw new InvalidOperationException($"Código '{dto.Codigo}' já existe nesta entidade.");

        escola.Codigo = dto.Codigo;
        escola.Nome = dto.Nome;
        escola.Endereco = dto.Endereco;
        escola.Bairro = dto.Bairro;
        escola.Cidade = dto.Cidade;
        escola.Uf = dto.Uf;
        escola.Cep = dto.Cep;
        escola.Email = dto.Email;
        escola.Obs = dto.Obs;
        escola.EntidadeId = dto.EntidadeId;

        await db.SaveChangesAsync();
        return await GetByIdAsync(id, escritorioId);
    }

    public async Task DeleteAsync(Guid id, Guid escritorioId)
    {
        var escola = await db.Escolas
            .Include(e => e.Entidade)
            .FirstOrDefaultAsync(e => e.Id == id && e.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Escola {id} não encontrada.");

        db.Escolas.Remove(escola);
        await db.SaveChangesAsync();
    }

    private static EscolaDto ToDto(Escola e) => new()
    {
        Id = e.Id,
        Codigo = e.Codigo,
        Nome = e.Nome,
        Endereco = e.Endereco,
        Bairro = e.Bairro,
        Cidade = e.Cidade,
        Uf = e.Uf,
        Cep = e.Cep,
        Email = e.Email,
        Obs = e.Obs,
        EntidadeId = e.EntidadeId,
        EntidadeNome = e.Entidade?.Nome
    };
}
