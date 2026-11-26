using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Entidade;

namespace CobrancaEscolar.Services.Interfaces;

public interface IEntidadeService
{
    Task<PagedResultDto<EntidadeDto>> GetAllAsync(EntidadeFilterDto filter, Guid escritorioId);
    Task<EntidadeDto> GetByIdAsync(Guid id, Guid escritorioId);
    Task<EntidadeDto> CreateAsync(CreateEntidadeDto dto, Guid escritorioId);
    Task<EntidadeDto> UpdateAsync(Guid id, UpdateEntidadeDto dto, Guid escritorioId);
    Task DeleteAsync(Guid id, Guid escritorioId);
    Task<IEnumerable<EntidadeDto>> GetComboAsync(Guid escritorioId);
}
