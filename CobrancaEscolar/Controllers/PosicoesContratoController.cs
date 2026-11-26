using CobrancaEscolar.Common;
using CobrancaEscolar.DTOs.PosicaoContrato;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CobrancaEscolar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PosicoesContratoController(
    IEscritorioScopedService<PosicaoContratoDto, CreatePosicaoContratoDto, UpdatePosicaoContratoDto, PosicaoContratoFilterDto> service)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PosicaoContratoFilterDto filter)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.GetAllAsync(filter, escritorioId));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        return Ok(await service.GetByIdAsync(id, escritorioId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePosicaoContratoDto dto)
    {
        var escritorioId = User.GetEscritorioId()
            ?? throw new UnauthorizedAccessException("Usuário não está associado a um escritório.");
        var result = await service.CreateAsync(dto, escritorioId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePosicaoContratoDto dto)
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
