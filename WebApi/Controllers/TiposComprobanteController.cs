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
    public class TiposComprobanteController : ControllerBase
    {
        private readonly ITipoComprobanteBusiness _tipoComprobanteBusiness;

        public TiposComprobanteController(ITipoComprobanteBusiness tipoComprobanteBusiness)
        {
            _tipoComprobanteBusiness = tipoComprobanteBusiness;
        }

        [HttpGet("obtener-todos")]
        public async Task<ActionResult<List<TipoComprobante>>> GetAllTiposComprobante()
        {
            var datos = await _tipoComprobanteBusiness.GetAll();
            return datos.ToList();
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<TipoComprobante>> GetTipoComprobanteById(string id)
        {
            var tipoComprobante = await _tipoComprobanteBusiness.Get(id);
            if (tipoComprobante == null) return NotFound();
            return tipoComprobante;
        }

        [HttpGet("obtener-por-codigo/{codigo}")]
        public async Task<ActionResult<TipoComprobante>> GetTipoComprobanteByCodigo(string codigo)
        {
            var tipoComprobante = await _tipoComprobanteBusiness.GetByCodigo(codigo);
            if (tipoComprobante == null) return NotFound();
            return tipoComprobante;
        }

        [HttpGet("obtener-por-abreviatura/{abreviatura}")]
        public async Task<ActionResult<TipoComprobante>> GetTipoComprobanteByAbreviatura(string abreviatura)
        {
            var tipoComprobante = await _tipoComprobanteBusiness.GetByAbreviatura(abreviatura);
            if (tipoComprobante == null) return NotFound();
            return tipoComprobante;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<TipoComprobante>> CreateTipoComprobante(TipoComprobante tipoComprobante)
        {
            await _tipoComprobanteBusiness.Add(tipoComprobante);
            return CreatedAtAction(nameof(GetTipoComprobanteById), new { id = tipoComprobante.Id }, tipoComprobante);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateTipoComprobante(TipoComprobante tipoComprobante)
        {
            await _tipoComprobanteBusiness.Update(tipoComprobante);
            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteTipoComprobante(string id)
        {
            await _tipoComprobanteBusiness.Delete(id);
            return NoContent();
        }
    }
}
