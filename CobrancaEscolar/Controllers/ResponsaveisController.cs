using CobrancaEscolar.Common;
using CobrancaEscolar.DTOs.Responsavel;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CobrancaEscolar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResponsaveisController(IResponsavelService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ResponsavelFilterDto filter)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.GetAllAsync(filter, escritorioId));
    }

    /// <summary>Busca rápida por nome ou CPF para campos de autocomplete (mínimo 3 caracteres).</summary>
    [HttpGet("autocomplete")]
    public async Task<IActionResult> Autocomplete([FromQuery] string termo, [FromQuery] Guid? entidadeId)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.AutocompleteAsync(termo ?? string.Empty, escritorioId, entidadeId));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.GetByIdAsync(id, escritorioId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateResponsavelDto dto)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        var result = await service.CreateAsync(dto, escritorioId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateResponsavelDto dto)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.UpdateAsync(id, dto, escritorioId));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        await service.DeleteAsync(id, escritorioId);
        return NoContent();
    }
}
