using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Banco;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.Entities;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class BancoService(AppDbContext db)
{
    public async Task<PagedResultDto<BancoDto>> GetAllAsync(BancoFilterDto filter)
    {
        var query = db.Bancos.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(b => b.Nome.Contains(filter.Nome));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(b => b.Nome)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(b => ToDto(b))
            .ToListAsync();

        return new PagedResultDto<BancoDto> { Items = items, TotalCount = total, Page = filter.Page, PageSize = filter.PageSize };
    }

    public async Task<BancoDto> GetByIdAsync(int id)
    {
        var b = await db.Bancos.FindAsync(id) ?? throw new KeyNotFoundException($"Banco {id} não encontrado.");
        return ToDto(b);
    }

    public async Task<BancoDto> CreateAsync(CreateBancoDto dto)
    {
        if (await db.Bancos.AnyAsync(b => b.Nome == dto.Nome))
            throw new InvalidOperationException($"Banco '{dto.Nome}' já existe.");

        var b = new Banco { Nome = dto.Nome };
        db.Bancos.Add(b);
        await db.SaveChangesAsync();
        return ToDto(b);
    }

    public async Task<BancoDto> UpdateAsync(int id, UpdateBancoDto dto)
    {
        var b = await db.Bancos.FindAsync(id) ?? throw new KeyNotFoundException($"Banco {id} não encontrado.");
        if (await db.Bancos.AnyAsync(x => x.Nome == dto.Nome && x.Id != id))
            throw new InvalidOperationException($"Banco '{dto.Nome}' já existe.");

        b.Nome = dto.Nome;
        await db.SaveChangesAsync();
        return ToDto(b);
    }

    public async Task DeleteAsync(int id)
    {
        var b = await db.Bancos.FindAsync(id) ?? throw new KeyNotFoundException($"Banco {id} não encontrado.");
        db.Bancos.Remove(b);
        await db.SaveChangesAsync();
    }

    private static BancoDto ToDto(Banco b) => new() { Id = b.Id, Nome = b.Nome };
}
