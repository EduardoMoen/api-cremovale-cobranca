using CobrancaEscolar.Common;
using CobrancaEscolar.DTOs.Auth;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CobrancaEscolar.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Realiza login e retorna o token JWT.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        var result = await authService.LoginAsync(dto);
        return Ok(result);
    }

    /// <summary>Troca a senha do usuário autenticado.</summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.GetUserId();
        await authService.ChangePasswordAsync(userId, dto);
        return NoContent();
    }

    /// <summary>Define a senha no primeiro acesso (usuário sem senha).</summary>
    [HttpPost("set-initial-password")]
    [AllowAnonymous]
    public async Task<IActionResult> SetInitialPassword([FromQuery] int userId, [FromBody] SetInitialPasswordDto dto)
    {
        await authService.SetInitialPasswordAsync(userId, dto);
        return NoContent();
    }

    /// <summary>Verifica se o usuário precisa definir senha (primeiro acesso).</summary>
    [HttpGet("requires-password-setup/{userId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> RequiresPasswordSetup(int userId)
    {
        var result = await authService.RequiresPasswordSetupAsync(userId);
        return Ok(result);
    }
}
