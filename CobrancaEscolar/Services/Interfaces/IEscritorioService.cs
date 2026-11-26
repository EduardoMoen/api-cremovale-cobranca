using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Escritorio;

namespace CobrancaEscolar.Services.Interfaces;

public interface IEscritorioService
{
    Task<PagedResultDto<EscritorioDto>> GetAllAsync(EscritorioFilterDto filter);
    Task<EscritorioDto> GetByIdAsync(Guid id);
    Task<EscritorioDto> CreateAsync(CreateEscritorioDto dto);
    Task<EscritorioDto> UpdateAsync(Guid id, UpdateEscritorioDto dto);
    Task DeleteAsync(Guid id);
}
