using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentaBusiness _ventaBusiness;
        private readonly IVentaDetalleBusiness _ventaDetalleBusiness;

        public VentasController(IVentaBusiness ventaBusiness, IVentaDetalleBusiness ventaDetalleBusiness)
        {
            _ventaBusiness = ventaBusiness;
            _ventaDetalleBusiness = ventaDetalleBusiness;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerVenta(string id)
        {
            var venta = await _ventaBusiness.Get(id);
            if (venta == null)
            {
                return NotFound();
            }

            venta.Detalles = (await _ventaDetalleBusiness.ObtenerDetallesPorVenta(id)).ToList();

            return Ok(venta);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ventas = await _ventaBusiness.GetAll();
            return Ok(ventas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Venta venta)
        {
            if (venta == null || venta.Id != id)
            {
                return BadRequest();
            }

            await _ventaBusiness.Update(venta);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _ventaBusiness.Delete(id);
            return NoContent();
        }
    }
}
