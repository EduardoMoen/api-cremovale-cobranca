using CobrancaEscolar.Common;
using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Usuario;
using CobrancaEscolar.Entities;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CobrancaEscolar.Services;

public class UsuarioService(AppDbContext db) : IUsuarioService
{
    public async Task<PagedResultDto<UsuarioDto>> GetAllAsync(UsuarioFilterDto filter)
    {
        var query = db.Usuarios.Include(u => u.Escritorio).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Username))
            query = query.Where(u => u.Username.Contains(filter.Username));

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(u => u.Email.Contains(filter.Email));

        if (filter.IsActive.HasValue)
            query = query.Where(u => u.IsActive == filter.IsActive.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(u => u.Username)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(u => ToDto(u))
            .ToListAsync();

        return new PagedResultDto<UsuarioDto>
        {
            Items = items,
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<UsuarioDto> GetByIdAsync(int id)
    {
        var usuario = await db.Usuarios.Include(u => u.Escritorio).FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado.");

        return ToDto(usuario);
    }

    public async Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto)
    {
        if (await db.Usuarios.AnyAsync(u => u.Username == dto.Username))
            throw new InvalidOperationException($"Username '{dto.Username}' já está em uso.");

        if (await db.Usuarios.AnyAsync(u => u.Email == dto.Email))
            throw new InvalidOperationException($"E-mail '{dto.Email}' já está em uso.");

        var usuario = new Usuario
        {
            Username = dto.Username,
            Password = PasswordHelper.NoPasswordHash, // Sem senha: primeiro acesso
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            IsSuperuser = dto.IsSuperuser,
            IsStaff = dto.IsStaff,
            EscritorioId = dto.EscritorioId,
            IsActive = true,
            DateJoined = DateTime.UtcNow
        };

        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync();

        return await GetByIdAsync(usuario.Id);
    }

    public async Task<UsuarioDto> UpdateAsync(int id, UpdateUsuarioDto dto)
    {
        var usuario = await db.Usuarios.FindAsync(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado.");

        if (await db.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id))
            throw new InvalidOperationException($"E-mail '{dto.Email}' já está em uso.");

        usuario.FirstName = dto.FirstName;
        usuario.LastName = dto.LastName;
        usuario.Email = dto.Email;
        usuario.IsSuperuser = dto.IsSuperuser;
        usuario.IsStaff = dto.IsStaff;
        usuario.IsActive = dto.IsActive;
        usuario.EscritorioId = dto.EscritorioId;

        await db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await db.Usuarios.FindAsync(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado.");

        db.Usuarios.Remove(usuario);
        await db.SaveChangesAsync();
    }

    public async Task ToggleActiveAsync(int id, bool isActive)
    {
        var usuario = await db.Usuarios.FindAsync(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado.");

        usuario.IsActive = isActive;
        await db.SaveChangesAsync();
    }

    private static UsuarioDto ToDto(Usuario u) => new()
    {
        Id = u.Id,
        Username = u.Username,
        FirstName = u.FirstName,
        LastName = u.LastName,
        Email = u.Email,
        IsSuperuser = u.IsSuperuser,
        IsStaff = u.IsStaff,
        IsActive = u.IsActive,
        DateJoined = u.DateJoined,
        LastLogin = u.LastLogin,
        EscritorioId = u.EscritorioId,
        EscritorioNome = u.Escritorio?.Nome
    };
}
