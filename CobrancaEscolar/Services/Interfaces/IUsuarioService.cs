using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Usuario;

namespace CobrancaEscolar.Services.Interfaces;

public interface IUsuarioService
{
    Task<PagedResultDto<UsuarioDto>> GetAllAsync(UsuarioFilterDto filter);
    Task<UsuarioDto> GetByIdAsync(int id);
    Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto);
    Task<UsuarioDto> UpdateAsync(int id, UpdateUsuarioDto dto);
    Task DeleteAsync(int id);
    Task ToggleActiveAsync(int id, bool isActive);
}
