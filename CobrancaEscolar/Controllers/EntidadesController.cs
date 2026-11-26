using CobrancaEscolar.Common;
using CobrancaEscolar.DTOs.Entidade;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CobrancaEscolar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EntidadesController(IEntidadeService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] EntidadeFilterDto filter)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.GetAllAsync(filter, escritorioId));
    }

    [HttpGet("combo")]
    public async Task<IActionResult> GetCombo()
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.GetComboAsync(escritorioId));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.GetByIdAsync(id, escritorioId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEntidadeDto dto)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        var result = await service.CreateAsync(dto, escritorioId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEntidadeDto dto)
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
