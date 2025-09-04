using Business.Interfaces;
using Entities.DTOs;
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
    }
}
