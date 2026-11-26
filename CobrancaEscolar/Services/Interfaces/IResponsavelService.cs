using CobrancaEscolar.DTOs.Common;
using CobrancaEscolar.DTOs.Responsavel;

namespace CobrancaEscolar.Services.Interfaces;

public interface IResponsavelService
{
    Task<PagedResultDto<ResponsavelDto>> GetAllAsync(ResponsavelFilterDto filter, Guid escritorioId);
    Task<ResponsavelDto> GetByIdAsync(Guid id, Guid escritorioId);
    Task<ResponsavelDto> CreateAsync(CreateResponsavelDto dto, Guid escritorioId);
    Task<ResponsavelDto> UpdateAsync(Guid id, UpdateResponsavelDto dto, Guid escritorioId);
    Task DeleteAsync(Guid id, Guid escritorioId);
    Task<IReadOnlyList<ResponsavelAutocompleteDto>> AutocompleteAsync(string termo, Guid escritorioId, Guid? entidadeId);
}
