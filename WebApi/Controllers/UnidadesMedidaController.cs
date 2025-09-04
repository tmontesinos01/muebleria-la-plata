using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnidadesMedidaController : ControllerBase
    {
        private readonly IUnidadMedidaBusiness _unidadMedidaBusiness;

        public UnidadesMedidaController(IUnidadMedidaBusiness unidadMedidaBusiness)
        {
            _unidadMedidaBusiness = unidadMedidaBusiness;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<List<UnidadMedida>>> GetAllUnidadesMedida()
        {
            var unidadesMedida = await _unidadMedidaBusiness.GetAll();
            return unidadesMedida.ToList();
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<UnidadMedida>> GetUnidadMedidaById(string id)
        {
            var unidadMedida = await _unidadMedidaBusiness.Get(id);
            if (unidadMedida == null) return NotFound();
            return unidadMedida;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<UnidadMedida>> CreateUnidadMedida(UnidadMedida unidadMedida)
        {
            await _unidadMedidaBusiness.Add(unidadMedida);
            return CreatedAtAction(nameof(GetUnidadMedidaById), new { id = unidadMedida.Id }, unidadMedida);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateUnidadMedida(UnidadMedida unidadMedida)
        {
            await _unidadMedidaBusiness.Update(unidadMedida);
            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteUnidadMedida(string id)
        {
            await _unidadMedidaBusiness.Delete(id);
            return NoContent();
        }
    }
}
