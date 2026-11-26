using CobrancaEscolar.DTOs.Common;

namespace CobrancaEscolar.Services.Interfaces;

/// <summary>
/// Interface genérica para serviços com escopo de escritório:
/// Andamento, Lugar, PosicaoCheque, PosicaoContrato.
/// </summary>
public interface IEscritorioScopedService<TDto, TCreateDto, TUpdateDto, TFilterDto>
{
    Task<PagedResultDto<TDto>> GetAllAsync(TFilterDto filter, Guid escritorioId);
    Task<TDto> GetByIdAsync(Guid id, Guid escritorioId);
    Task<TDto> CreateAsync(TCreateDto dto, Guid escritorioId);
    Task<TDto> UpdateAsync(Guid id, TUpdateDto dto, Guid escritorioId);
    Task DeleteAsync(Guid id, Guid escritorioId);
}
