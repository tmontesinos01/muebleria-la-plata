using Entities.DTOs.Facturacion;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFacturacionBusiness
    {
        Task<EmitirFacturaResponseDTO> EmitirFactura(EmitirFacturaRequestDTO request);
        Task<EmitirFacturaResponseDTO> EmitirFacturaDesdeVenta(string ventaId, EmitirFacturaRequestDTO request);
        Task<AnularFacturaResponseDTO> AnularFactura(string facturaId, AnularFacturaRequestDTO request);
        Task<ReimprimirFacturaResponseDTO> ReimprimirFactura(string facturaId, ReimprimirFacturaRequestDTO request);
    }
}
