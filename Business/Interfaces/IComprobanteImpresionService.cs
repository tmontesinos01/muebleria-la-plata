using Entities.DTOs.Facturacion;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IComprobanteImpresionService
    {
        /// <summary>
        /// Reimprime una factura usando TusFacturasAPP
        /// </summary>
        /// <param name="facturaId">ID de la factura a reimprimir</param>
        /// <param name="request">Datos de la reimpresión</param>
        /// <returns>Resultado de la reimpresión</returns>
        Task<ReimprimirFacturaResponseDTO> ReimprimirFacturaAsync(string facturaId, ReimprimirFacturaRequestDTO request);

        /// <summary>
        /// Reimprime una nota de crédito/débito
        /// </summary>
        /// <param name="notaId">ID de la nota a reimprimir</param>
        /// <param name="request">Datos de la reimpresión</param>
        /// <returns>Resultado de la reimpresión</returns>
        Task<ReimprimirFacturaResponseDTO> ReimprimirNotaAsync(string notaId, ReimprimirFacturaRequestDTO request);

        /// <summary>
        /// Valida si un comprobante puede ser reimpreso
        /// </summary>
        /// <param name="comprobanteId">ID del comprobante</param>
        /// <param name="tipoComprobante">Tipo de comprobante (factura, nota)</param>
        /// <returns>True si puede ser reimpreso, false en caso contrario</returns>
        Task<bool> ValidarReimpresionAsync(string comprobanteId, string tipoComprobante);

        /// <summary>
        /// Obtiene la URL del PDF de un comprobante
        /// </summary>
        /// <param name="comprobanteId">ID del comprobante</param>
        /// <param name="tipoComprobante">Tipo de comprobante</param>
        /// <returns>URL del PDF o null si no está disponible</returns>
        Task<string> ObtenerUrlPdfAsync(string comprobanteId, string tipoComprobante);
    }
}
