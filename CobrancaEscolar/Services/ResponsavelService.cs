using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Responsavel;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class ResponsavelService(AppDbContext db) : IResponsavelService
{
    public async Task<PagedResultDto<ResponsavelDto>> GetAllAsync(ResponsavelFilterDto filter, Guid escritorioId)
    {
        // Multitenancy obrigatório: apenas responsáveis do escritório do usuário
        var query = db.Responsaveis
            .Include(r => r.Entidade)
            .Where(r => r.Entidade!.EscritorioId == escritorioId);

        if (filter.EntidadeId.HasValue)
            query = query.Where(r => r.EntidadeId == filter.EntidadeId.Value);

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(r => r.Nome.Contains(filter.Nome));

        if (!string.IsNullOrWhiteSpace(filter.Cpf))
            query = query.Where(r => r.Cpf.Contains(filter.Cpf));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(r => r.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(r => ToDto(r))
            .ToListAsync();

        return new PagedResultDto<ResponsavelDto>
        {
            Items = items,
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<ResponsavelDto> GetByIdAsync(Guid id, Guid escritorioId)
    {
        var responsavel = await db.Responsaveis
            .Include(r => r.Entidade)
            .FirstOrDefaultAsync(r => r.Id == id && r.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Responsável {id} não encontrado.");

        return ToDto(responsavel);
    }

    public async Task<ResponsavelDto> CreateAsync(CreateResponsavelDto dto, Guid escritorioId)
    {
        // Garante que a entidade pertence ao escritório do usuário
        var entidade = await db.Entidades
            .FirstOrDefaultAsync(e => e.Id == dto.EntidadeId && e.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException("Entidade não encontrada para este escritório.");

        if (await db.Responsaveis.AnyAsync(r => r.Cpf == dto.Cpf && r.EntidadeId == dto.EntidadeId))
            throw new InvalidOperationException($"CPF '{dto.Cpf}' já cadastrado nesta entidade.");

        var responsavel = new Responsavel
        {
            Id = Guid.NewGuid(),
            Cpf = dto.Cpf,
            Nome = dto.Nome,
            Nascimento = dto.Nascimento,
            Endereco = dto.Endereco,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Uf = dto.Uf,
            Cep = dto.Cep,
            Rg = dto.Rg,
            RgEmissao = dto.RgEmissao,
            EstadoCivil = dto.EstadoCivil,
            Telefones = dto.Telefones,
            Email = dto.Email,
            EntidadeId = dto.EntidadeId,
            UltimaAlteracao = DateTime.UtcNow
        };

        db.Responsaveis.Add(responsavel);
        await db.SaveChangesAsync();
        return await GetByIdAsync(responsavel.Id, escritorioId);
    }

    public async Task<ResponsavelDto> UpdateAsync(Guid id, UpdateResponsavelDto dto, Guid escritorioId)
    {
        var responsavel = await db.Responsaveis
            .Include(r => r.Entidade)
            .FirstOrDefaultAsync(r => r.Id == id && r.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Responsável {id} não encontrado.");

        var entidade = await db.Entidades
            .FirstOrDefaultAsync(e => e.Id == dto.EntidadeId && e.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException("Entidade não encontrada para este escritório.");

        if (await db.Responsaveis.AnyAsync(r => r.Cpf == dto.Cpf && r.EntidadeId == dto.EntidadeId && r.Id != id))
            throw new InvalidOperationException($"CPF '{dto.Cpf}' já cadastrado nesta entidade.");

        responsavel.Cpf = dto.Cpf;
        responsavel.Nome = dto.Nome;
        responsavel.Nascimento = dto.Nascimento;
        responsavel.Endereco = dto.Endereco;
        responsavel.Complemento = dto.Complemento;
        responsavel.Bairro = dto.Bairro;
        responsavel.Cidade = dto.Cidade;
        responsavel.Uf = dto.Uf;
        responsavel.Cep = dto.Cep;
        responsavel.Rg = dto.Rg;
        responsavel.RgEmissao = dto.RgEmissao;
        responsavel.EstadoCivil = dto.EstadoCivil;
        responsavel.Telefones = dto.Telefones;
        responsavel.Email = dto.Email;
        responsavel.EntidadeId = dto.EntidadeId;
        responsavel.UltimaAlteracao = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return await GetByIdAsync(id, escritorioId);
    }

    public async Task DeleteAsync(Guid id, Guid escritorioId)
    {
        var responsavel = await db.Responsaveis
            .Include(r => r.Entidade)
            .FirstOrDefaultAsync(r => r.Id == id && r.Entidade!.EscritorioId == escritorioId)
            ?? throw new KeyNotFoundException($"Responsável {id} não encontrado.");

        db.Responsaveis.Remove(responsavel);
        await db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<ResponsavelAutocompleteDto>> AutocompleteAsync(
        string termo, Guid escritorioId, Guid? entidadeId)
    {
        termo = termo.Trim();
        if (termo.Length < 3)
            return [];

        // Multitenancy obrigatório + projeção mínima (sem Include nem colunas desnecessárias)
        var query = db.Responsaveis
            .AsNoTracking()
            .Where(r => r.Entidade!.EscritorioId == escritorioId
                        && (r.Nome.Contains(termo) || r.Cpf.Contains(termo)));

        // Quando informada, restringe aos responsáveis da entidade
        if (entidadeId.HasValue)
            query = query.Where(r => r.EntidadeId == entidadeId.Value);

        return await query
            .OrderBy(r => r.Nome)
            .Take(15)
            .Select(r => new ResponsavelAutocompleteDto { Id = r.Id, Nome = r.Nome, Cpf = r.Cpf })
            .ToListAsync();
    }

    private static ResponsavelDto ToDto(Responsavel r) => new()
    {
        Id = r.Id,
        Cpf = r.Cpf,
        Nome = r.Nome,
        Nascimento = r.Nascimento,
        Endereco = r.Endereco,
        Complemento = r.Complemento,
        Bairro = r.Bairro,
        Cidade = r.Cidade,
        Uf = r.Uf,
        Cep = r.Cep,
        Rg = r.Rg,
        RgEmissao = r.RgEmissao,
        EstadoCivil = r.EstadoCivil,
        Telefones = r.Telefones,
        Email = r.Email,
        EntidadeId = r.EntidadeId,
        EntidadeNome = r.Entidade?.Nome,
        UltimaAlteracao = r.UltimaAlteracao
    };
}
