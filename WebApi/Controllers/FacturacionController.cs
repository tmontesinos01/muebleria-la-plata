using Business.Interfaces;
using Entities.DTOs.Facturacion;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturacionController : ControllerBase
    {
        private readonly IFacturacionBusiness _facturacionBusiness;

        public FacturacionController(IFacturacionBusiness facturacionBusiness)
        {
            _facturacionBusiness = facturacionBusiness;
        }

        [HttpPost("emitir-factura")]
        public async Task<ActionResult<EmitirFacturaResponseDTO>> EmitirFactura([FromBody] EmitirFacturaRequestDTO request)
        {
            var resultado = await _facturacionBusiness.EmitirFactura(request);
            
            if (resultado.exito)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado);
            }
        }

        [HttpPost("emitir-factura-desde-venta/{ventaId}")]
        public async Task<ActionResult<EmitirFacturaResponseDTO>> EmitirFacturaDesdeVenta(string ventaId, [FromBody] EmitirFacturaRequestDTO request)
        {
            var resultado = await _facturacionBusiness.EmitirFacturaDesdeVenta(ventaId, request);
            
            if (resultado.exito)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado);
            }
        }

        [HttpPost("anular-factura/{facturaId}")]
        public async Task<ActionResult<AnularFacturaResponseDTO>> AnularFactura(string facturaId, [FromBody] AnularFacturaRequestDTO request)
        {
            var resultado = await _facturacionBusiness.AnularFactura(facturaId, request);
            
            if (resultado.exito)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado);
            }
        }

        [HttpPost("reimprimir-factura/{facturaId}")]
        public async Task<ActionResult<ReimprimirFacturaResponseDTO>> ReimprimirFactura(string facturaId, [FromBody] ReimprimirFacturaRequestDTO request)
        {
            var resultado = await _facturacionBusiness.ReimprimirFactura(facturaId, request);
            
            if (resultado.exito)
            {
                // Si hay contenido PDF, devolverlo como archivo
                if (resultado.pdf_content != null && resultado.pdf_content.Length > 0)
                {
                    return File(resultado.pdf_content, "application/pdf", resultado.nombre_archivo);
                }
                else
                {
                    // Si no hay contenido PDF, devolver la respuesta con URL
                    return Ok(resultado);
                }
            }
            else
            {
                return BadRequest(resultado);
            }
        }
    }
}
