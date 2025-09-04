using Entities.DTOs;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFacturacionBusiness
    {
        Task<EmitirFacturaResponseDTO> EmitirFactura(EmitirFacturaRequestDTO request);
    }
}
