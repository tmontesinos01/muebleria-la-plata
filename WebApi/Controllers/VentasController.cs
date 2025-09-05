using Business.Interfaces;
using Entities;
using Entities.DTOs.Notas;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentaBusiness _ventaBusiness;
        private readonly IVentaDetalleBusiness _ventaDetalleBusiness;

        public VentasController(IVentaBusiness ventaBusiness, IVentaDetalleBusiness ventaDetalleBusiness)
        {
            _ventaBusiness = ventaBusiness;
            _ventaDetalleBusiness = ventaDetalleBusiness;
        }

        [HttpGet("obtener/{id}")]
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

        [HttpGet("obtener-todos")]
        public async Task<IActionResult> GetAll()
        {
            var ventas = await _ventaBusiness.GetAll();
            return Ok(ventas);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Venta venta)
        {
            if (venta == null || venta.Id != id)
            {
                return BadRequest();
            }

            await _ventaBusiness.Update(venta);
            return NoContent();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _ventaBusiness.Delete(id);
            return NoContent();
        }

        [HttpGet("pendientes-facturacion")]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentasPendientesFacturacion()
        {
            var ventas = await _ventaBusiness.GetVentasPendientesFacturacion();
            return Ok(ventas);
        }

        // Endpoints para notas de crédito/débito
        [HttpPost("emitir-nota-credito")]
        public async Task<ActionResult<EmitirNotaResponseDTO>> EmitirNotaCredito([FromBody] EmitirNotaRequestDTO request)
        {
            var resultado = await _ventaBusiness.EmitirNotaCredito(request);
            
            if (resultado.exito)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado);
            }
        }

        [HttpPost("emitir-nota-debito")]
        public async Task<ActionResult<EmitirNotaResponseDTO>> EmitirNotaDebito([FromBody] EmitirNotaRequestDTO request)
        {
            var resultado = await _ventaBusiness.EmitirNotaDebito(request);
            
            if (resultado.exito)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado);
            }
        }

        [HttpGet("consultar-notas-por-factura/{facturaId}")]
        public async Task<ActionResult<ConsultarNotasResponseDTO>> ConsultarNotasPorFactura(string facturaId)
        {
            var resultado = await _ventaBusiness.ConsultarNotasPorFactura(facturaId);
            
            if (resultado.exito)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado);
            }
        }

        [HttpPost("consultar-notas")]
        public async Task<ActionResult<ConsultarNotasResponseDTO>> ConsultarNotas([FromBody] ConsultarNotasRequestDTO request)
        {
            var resultado = await _ventaBusiness.ConsultarNotas(request);
            
            if (resultado.exito)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado);
            }
        }
    }
}
