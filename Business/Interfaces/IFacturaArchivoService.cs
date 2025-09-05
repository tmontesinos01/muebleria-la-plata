using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFacturaArchivoService
    {
        /// <summary>
        /// Descarga el PDF de una factura desde una URL
        /// </summary>
        /// <param name="urlPdf">URL del PDF de la factura</param>
        /// <returns>Contenido del PDF como array de bytes</returns>
        Task<byte[]> DescargarPdfFacturaAsync(string urlPdf);

        /// <summary>
        /// Valida si una URL de PDF es accesible
        /// </summary>
        /// <param name="urlPdf">URL del PDF a validar</param>
        /// <returns>True si la URL es accesible, false en caso contrario</returns>
        Task<bool> ValidarUrlPdfAsync(string urlPdf);

        /// <summary>
        /// Genera un nombre de archivo único para una factura
        /// </summary>
        /// <param name="numeroFactura">Número de la factura</param>
        /// <param name="tipoComprobante">Tipo de comprobante</param>
        /// <param name="sufijo">Sufijo opcional (ej: "reimpresion")</param>
        /// <returns>Nombre de archivo generado</returns>
        string GenerarNombreArchivo(string numeroFactura, string tipoComprobante, string sufijo = null);

        /// <summary>
        /// Convierte contenido base64 a array de bytes
        /// </summary>
        /// <param name="base64Content">Contenido en base64</param>
        /// <returns>Array de bytes del contenido</returns>
        byte[] ConvertirBase64ABytes(string base64Content);
    }
}
