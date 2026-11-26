using CobrancaEscolar.DTOs.Common;

namespace CobrancaEscolar.Services.Interfaces;

/// <summary>
/// Interface genérica para serviços de tabelas de domínio simples (nome + id).
/// Usado por Alinea, Banco, TipoCobranca, Indice.
/// </summary>
public interface IGenericDomainService<TDto, TCreateDto, TUpdateDto, TFilterDto>
{
    Task<PagedResultDto<TDto>> GetAllAsync(TFilterDto filter);
    Task<TDto> GetByIdAsync(int id);
    Task<TDto> CreateAsync(TCreateDto dto);
    Task<TDto> UpdateAsync(int id, TUpdateDto dto);
    Task DeleteAsync(int id);
}
