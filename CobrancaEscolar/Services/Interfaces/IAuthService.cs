using CobrancaEscolar.DTOs.Auth;

namespace CobrancaEscolar.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    Task SetInitialPasswordAsync(int userId, SetInitialPasswordDto dto);
    Task<bool> RequiresPasswordSetupAsync(int userId);
}
