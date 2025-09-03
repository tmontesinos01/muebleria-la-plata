using Business.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesMedidaController : ControllerBase
    {
        private readonly UnidadMedidaBusiness _unidadMedidaBusiness;

        public UnidadesMedidaController(UnidadMedidaBusiness unidadMedidaBusiness)
        {
            _unidadMedidaBusiness = unidadMedidaBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<List<UnidadMedida>>> GetAllUnidadesMedida()
        {
            return await _unidadMedidaBusiness.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UnidadMedida>> GetUnidadMedidaById(string id)
        {
            var unidadMedida = await _unidadMedidaBusiness.GetById(id);
            if (unidadMedida == null) return NotFound();
            return unidadMedida;
        }

        [HttpPost]
        public async Task<ActionResult<UnidadMedida>> CreateUnidadMedida(UnidadMedida unidadMedida)
        {
            await _unidadMedidaBusiness.Create(unidadMedida);
            return CreatedAtAction(nameof(GetUnidadMedidaById), new { id = unidadMedida.Id }, unidadMedida);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnidadMedida(string id, UnidadMedida unidadMedida)
        {
            if (id != unidadMedida.Id) return BadRequest();
            await _unidadMedidaBusiness.Update(id, unidadMedida);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidadMedida(string id)
        {
            await _unidadMedidaBusiness.Delete(id);
            return NoContent();
        }
    }
}
