using Business.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiposComprobanteController : ControllerBase
    {
        private readonly TipoComprobanteBusiness _tipoComprobanteBusiness;

        public TiposComprobanteController(TipoComprobanteBusiness tipoComprobanteBusiness)
        {
            _tipoComprobanteBusiness = tipoComprobanteBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoComprobante>>> GetAllTiposComprobante()
        {
            return await _tipoComprobanteBusiness.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoComprobante>> GetTipoComprobanteById(string id)
        {
            var tipoComprobante = await _tipoComprobanteBusiness.GetById(id);
            if (tipoComprobante == null) return NotFound();
            return tipoComprobante;
        }

        [HttpPost]
        public async Task<ActionResult<TipoComprobante>> CreateTipoComprobante(TipoComprobante tipoComprobante)
        {
            await _tipoComprobanteBusiness.Create(tipoComprobante);
            return CreatedAtAction(nameof(GetTipoComprobanteById), new { id = tipoComprobante.Id }, tipoComprobante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoComprobante(string id, TipoComprobante tipoComprobante)
        {
            if (id != tipoComprobante.Id) return BadRequest();
            await _tipoComprobanteBusiness.Update(id, tipoComprobante);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoComprobante(string id)
        {
            await _tipoComprobanteBusiness.Delete(id);
            return NoContent();
        }
    }
}
