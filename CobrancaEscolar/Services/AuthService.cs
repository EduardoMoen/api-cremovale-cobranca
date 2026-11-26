using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CobrancaEscolar.Common;
using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Auth;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CobrancaEscolar.Services;

public class AuthService(AppDbContext db, IConfiguration config) : IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var usuario = await db.Usuarios
            .FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);

        if (usuario is null)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        // Usuário sem senha precisa definir uma antes de logar
        if (!PasswordHelper.HasPassword(usuario.Password))
            throw new InvalidOperationException("REQUIRES_PASSWORD_SETUP");

        if (!PasswordHelper.Verify(dto.Password, usuario.Password))
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        usuario.LastLogin = DateTime.UtcNow;
        await db.SaveChangesAsync();

        var token = GenerateToken(usuario);
        var expiresAt = DateTime.UtcNow.AddHours(8);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            Username = usuario.Username,
            FullName = $"{usuario.FirstName} {usuario.LastName}".Trim(),
            IsSuperuser = usuario.IsSuperuser,
            EscritorioId = usuario.EscritorioId
        };
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var usuario = await db.Usuarios.FindAsync(userId)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");

        if (!PasswordHelper.HasPassword(usuario.Password))
            throw new InvalidOperationException("REQUIRES_PASSWORD_SETUP");

        if (!PasswordHelper.Verify(dto.CurrentPassword, usuario.Password))
            throw new UnauthorizedAccessException("Senha atual inválida.");

        if (dto.NewPassword != dto.ConfirmNewPassword)
            throw new ArgumentException("As senhas não conferem.");

        usuario.Password = PasswordHelper.Hash(dto.NewPassword);
        await db.SaveChangesAsync();
    }

    public async Task SetInitialPasswordAsync(int userId, SetInitialPasswordDto dto)
    {
        var usuario = await db.Usuarios.FindAsync(userId)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");

        if (PasswordHelper.HasPassword(usuario.Password))
            throw new InvalidOperationException("Usuário já possui senha definida. Use a troca de senha.");

        if (dto.NewPassword != dto.ConfirmNewPassword)
            throw new ArgumentException("As senhas não conferem.");

        usuario.Password = PasswordHelper.Hash(dto.NewPassword);
        await db.SaveChangesAsync();
    }

    public async Task<bool> RequiresPasswordSetupAsync(int userId)
    {
        var usuario = await db.Usuarios.FindAsync(userId)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");

        return !PasswordHelper.HasPassword(usuario.Password);
    }

    private string GenerateToken(Entities.Usuario usuario)
    {
        var jwtKey = config["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key não configurada.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Username),
            new(ClaimTypes.Email, usuario.Email),
            new("is_superuser", usuario.IsSuperuser.ToString().ToLower()),
        };

        if (usuario.EscritorioId.HasValue)
            claims.Add(new Claim("escritorio_id", usuario.EscritorioId.Value.ToString()));

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
