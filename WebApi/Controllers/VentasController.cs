using Business.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly VentaBusiness _ventaBusiness;
        private readonly VentaDetalleBusiness _ventaDetalleBusiness;

        public VentasController(VentaBusiness ventaBusiness, VentaDetalleBusiness ventaDetalleBusiness)
        {
            _ventaBusiness = ventaBusiness;
            _ventaDetalleBusiness = ventaDetalleBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> CrearVenta([FromBody] Venta venta)
        {
            if (venta == null)
            {
                return BadRequest("Venta no puede ser nulo.");
            }

            var ventaId = await _ventaBusiness.CrearVenta(venta);
            return Ok(new { VentaId = ventaId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVenta(string id)
        {
            var venta = await _ventaBusiness.ObtenerVentaPorId(id);
            if (venta == null)
            {
                return NotFound();
            }

            venta.Detalles = (await _ventaDetalleBusiness.ObtenerDetallesPorVenta(id)).ToList();

            return Ok(venta);
        }
    }
}
