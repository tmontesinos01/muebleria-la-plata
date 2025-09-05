using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaBusiness _facturaBusiness;

        public FacturasController(IFacturaBusiness facturaBusiness)
        {
            _facturaBusiness = facturaBusiness;
        }

        [HttpGet("obtener-todas")]
        public async Task<ActionResult<IEnumerable<Factura>>> GetAllFacturas()
        {
            var facturas = await _facturaBusiness.GetAll();
            return Ok(facturas);
        }

        [HttpGet("obtener/{id}")]
        public async Task<ActionResult<Factura>> GetFacturaById(string id)
        {
            var factura = await _facturaBusiness.Get(id);
            if (factura == null) return NotFound();
            return Ok(factura);
        }

        [HttpGet("por-venta/{ventaId}")]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturasPorVenta(string ventaId)
        {
            var facturas = await _facturaBusiness.GetFacturasPorVenta(ventaId);
            return Ok(facturas);
        }

        [HttpGet("por-estado/{estado}")]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturasPorEstado(string estado)
        {
            var facturas = await _facturaBusiness.GetFacturasPorEstado(estado);
            return Ok(facturas);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteFactura(string id)
        {
            await _facturaBusiness.Delete(id);
            return NoContent();
        }
    }
}
