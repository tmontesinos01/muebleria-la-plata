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
    public class EstadosFacturaController : ControllerBase
    {
        private readonly IEstadoFacturaBusiness _estadoFacturaBusiness;

        public EstadosFacturaController(IEstadoFacturaBusiness estadoFacturaBusiness)
        {
            _estadoFacturaBusiness = estadoFacturaBusiness;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<List<EstadoFactura>>> GetAllEstadosFactura()
        {
            var datos = await _estadoFacturaBusiness.GetAll();
            return datos.ToList();
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<EstadoFactura>> GetEstadoFacturaById(string id)
        {
            var estadoFactura = await _estadoFacturaBusiness.Get(id);
            if (estadoFactura == null) return NotFound();
            return estadoFactura;
        }

        [HttpGet("obtener-por-codigo/{codigo}")]
        public async Task<ActionResult<EstadoFactura>> GetEstadoFacturaByCodigo(string codigo)
        {
            var estadoFactura = await _estadoFacturaBusiness.GetByCodigo(codigo);
            if (estadoFactura == null) return NotFound();
            return estadoFactura;
        }

        [HttpGet("que-permiten-anulacion")]
        public async Task<ActionResult<List<EstadoFactura>>> GetEstadosQuePermitenAnulacion()
        {
            var datos = await _estadoFacturaBusiness.GetEstadosQuePermitenAnulacion();
            return datos.ToList();
        }

        [HttpGet("estados-finales")]
        public async Task<ActionResult<List<EstadoFactura>>> GetEstadosFinales()
        {
            var datos = await _estadoFacturaBusiness.GetEstadosFinales();
            return datos.ToList();
        }

        [HttpPost("crear")]
        public async Task<ActionResult<EstadoFactura>> CreateEstadoFactura(EstadoFactura estadoFactura)
        {
            var newId = await _estadoFacturaBusiness.Add(estadoFactura);
            estadoFactura.Id = newId;
            return CreatedAtAction(nameof(GetEstadoFacturaById), new { id = newId }, estadoFactura);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateEstadoFactura(string id, EstadoFactura estadoFactura)
        {
            if (id != estadoFactura.Id) return BadRequest();
            await _estadoFacturaBusiness.Update(estadoFactura);
            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteEstadoFactura(string id)
        {
            await _estadoFacturaBusiness.Delete(id);
            return NoContent();
        }
    }
}
