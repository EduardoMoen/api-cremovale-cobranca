using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Escola;

namespace CobrancaEscolar.Services.Interfaces;

public interface IEscolaService
{
    Task<PagedResultDto<EscolaDto>> GetAllAsync(EscolaFilterDto filter, Guid escritorioId);
    Task<EscolaDto> GetByIdAsync(Guid id, Guid escritorioId);
    Task<EscolaDto> CreateAsync(CreateEscolaDto dto, Guid escritorioId);
    Task<EscolaDto> UpdateAsync(Guid id, UpdateEscolaDto dto, Guid escritorioId);
    Task DeleteAsync(Guid id, Guid escritorioId);
}
