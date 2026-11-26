using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Divida;

namespace CobrancaEscolar.Services.Interfaces;

public interface IDividaService
{
    Task<PagedResultDto<DividaListItemDto>> GetAllAsync(DividaFilterDto filter, Guid escritorioId);
    Task<DividaDto> GetByIdAsync(Guid id, Guid escritorioId);
    Task<DividaDto> CreateAsync(DividaSaveDto dto, Guid escritorioId, int usuarioId);
    Task<DividaDto> UpdateAsync(Guid id, DividaSaveDto dto, Guid escritorioId, int usuarioId);
    Task DeleteAsync(Guid id, Guid escritorioId);
}
