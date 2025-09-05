using Data.Interfaces;
using Entities;
using Entities.DTOs.Notas;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IVentaBusiness : IRepository<Venta>
    {
        public int CrearVenta(Venta datos);
        public Venta ObtenerVentaPorId(string id);
        Task<IEnumerable<Venta>> GetVentasPendientesFacturacion();
        Task<bool> MarcarVentaComoFacturada(string ventaId, string numeroFactura);
        
        // Métodos para notas de crédito/débito
        Task<EmitirNotaResponseDTO> EmitirNotaCredito(EmitirNotaRequestDTO request);
        Task<EmitirNotaResponseDTO> EmitirNotaDebito(EmitirNotaRequestDTO request);
        Task<ConsultarNotasResponseDTO> ConsultarNotasPorFactura(string facturaId);
        Task<ConsultarNotasResponseDTO> ConsultarNotas(ConsultarNotasRequestDTO request);
    }
}
