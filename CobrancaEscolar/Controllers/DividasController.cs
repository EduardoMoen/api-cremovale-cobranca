using CobrancaEscolar.Common;
using CobrancaEscolar.DTOs.Divida;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CobrancaEscolar.Controllers;

/// <summary>
/// Acesso: usuário autenticado que possua escritorio_id (policy "TemEscritorio").
/// Quem não tem escritório recebe 403.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthPolicies.TemEscritorio)]
public class DividasController(IDividaService service) : ControllerBase
{
    private Guid EscritorioId => User.GetEscritorioId()!.Value;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DividaFilterDto filter)
        => Ok(await service.GetAllAsync(filter, EscritorioId));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await service.GetByIdAsync(id, EscritorioId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DividaSaveDto dto)
    {
        var result = await service.CreateAsync(dto, EscritorioId, User.GetUserId());
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DividaSaveDto dto)
        => Ok(await service.UpdateAsync(id, dto, EscritorioId, User.GetUserId()));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id, EscritorioId);
        return NoContent();
    }
}
