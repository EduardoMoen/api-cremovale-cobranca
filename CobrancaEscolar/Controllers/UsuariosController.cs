using CobrancaEscolar.DTOs.Usuario;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CobrancaEscolar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController(IUsuarioService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UsuarioFilterDto filter)
        => Ok(await service.GetAllAsync(filter));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto)
    {
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUsuarioDto dto)
        => Ok(await service.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:int}/ativar")]
    public async Task<IActionResult> Ativar(int id)
    {
        await service.ToggleActiveAsync(id, true);
        return NoContent();
    }

    [HttpPatch("{id:int}/desativar")]
    public async Task<IActionResult> Desativar(int id)
    {
        await service.ToggleActiveAsync(id, false);
        return NoContent();
    }
}
