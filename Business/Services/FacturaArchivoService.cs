using Business.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Services
{
    public class FacturaArchivoService : IFacturaArchivoService
    {
        private readonly HttpClient _httpClient;

        public FacturaArchivoService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<byte[]> DescargarPdfFacturaAsync(string urlPdf)
        {
            if (string.IsNullOrEmpty(urlPdf))
            {
                throw new ArgumentException("La URL del PDF no puede estar vacía", nameof(urlPdf));
            }

            try
            {
                var response = await _httpClient.GetAsync(urlPdf);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Error descargando el PDF desde {urlPdf}: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new InvalidOperationException($"Timeout descargando el PDF desde {urlPdf}: {ex.Message}", ex);
            }
        }

        public async Task<bool> ValidarUrlPdfAsync(string urlPdf)
        {
            if (string.IsNullOrEmpty(urlPdf))
            {
                return false;
            }

            try
            {
                var response = await _httpClient.GetAsync(urlPdf);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public string GenerarNombreArchivo(string numeroFactura, string tipoComprobante, string sufijo = null)
        {
            if (string.IsNullOrEmpty(numeroFactura))
            {
                throw new ArgumentException("El número de factura no puede estar vacío", nameof(numeroFactura));
            }

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var tipo = tipoComprobante?.ToLower() ?? "comprobante";
            
            var nombreBase = $"{tipo}_{numeroFactura}_{timestamp}";
            
            if (!string.IsNullOrEmpty(sufijo))
            {
                nombreBase += $"_{sufijo}";
            }

            return $"{nombreBase}.pdf";
        }

        public byte[] ConvertirBase64ABytes(string base64Content)
        {
            if (string.IsNullOrEmpty(base64Content))
            {
                throw new ArgumentException("El contenido base64 no puede estar vacío", nameof(base64Content));
            }

            try
            {
                return Convert.FromBase64String(base64Content);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("El contenido base64 no tiene un formato válido", ex);
            }
        }
    }
}
