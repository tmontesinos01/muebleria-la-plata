using Business.Interfaces;
using Entities;
using Entities.DTOs.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ComprobanteImpresionService : IComprobanteImpresionService
    {
        private readonly IFacturaBusiness _facturaBusiness;
        private readonly IConfiguracionBusiness _configuracionBusiness;
        private readonly IFacturaArchivoService _facturaArchivoService;
        private readonly HttpClient _httpClient;

        public ComprobanteImpresionService(
            IFacturaBusiness facturaBusiness,
            IConfiguracionBusiness configuracionBusiness,
            IFacturaArchivoService facturaArchivoService,
            HttpClient httpClient)
        {
            _facturaBusiness = facturaBusiness ?? throw new ArgumentNullException(nameof(facturaBusiness));
            _configuracionBusiness = configuracionBusiness ?? throw new ArgumentNullException(nameof(configuracionBusiness));
            _facturaArchivoService = facturaArchivoService ?? throw new ArgumentNullException(nameof(facturaArchivoService));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ReimprimirFacturaResponseDTO> ReimprimirFacturaAsync(string facturaId, ReimprimirFacturaRequestDTO request)
        {
            try
            {
                // Validar parámetros de entrada
                if (string.IsNullOrEmpty(facturaId) || request == null)
                {
                    return new ReimprimirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Datos de reimpresión inválidos",
                        errores = new List<string> { "ID de factura y datos de reimpresión son requeridos" }
                    };
                }

                // Validar que la factura puede ser reimpresa
                var puedeReimprimir = await ValidarReimpresionAsync(facturaId, "factura");
                if (!puedeReimprimir)
                {
                    return new ReimprimirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "La factura no puede ser reimpresa",
                        errores = new List<string> { "La factura no existe, está anulada o no tiene PDF disponible" }
                    };
                }

                // Obtener la factura
                var factura = await _facturaBusiness.Get(facturaId);
                if (factura == null)
                {
                    return new ReimprimirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Factura no encontrada",
                        errores = new List<string> { $"Factura con ID '{facturaId}' no existe" }
                    };
                }

                // Intentar reimpresión con TusFacturasAPP
                var resultadoTusFacturas = await ReimprimirConTusFacturasAsync(factura, request);
                if (resultadoTusFacturas.exito)
                {
                    // Actualizar auditoría
                    await ActualizarAuditoriaFacturaAsync(factura, request.usuario_reimpresion);
                    return resultadoTusFacturas;
                }

                // Fallback: usar PDF existente
                return await ReimprimirConPdfExistenteAsync(factura, request);
            }
            catch (Exception ex)
            {
                return new ReimprimirFacturaResponseDTO
                {
                    exito = false,
                    mensaje = "Error interno en la reimpresión",
                    errores = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ReimprimirFacturaResponseDTO> ReimprimirNotaAsync(string notaId, ReimprimirFacturaRequestDTO request)
        {
            // TODO: Implementar cuando se tenga la entidad Nota
            return new ReimprimirFacturaResponseDTO
            {
                exito = false,
                mensaje = "Reimpresión de notas no implementada aún",
                errores = new List<string> { "Esta funcionalidad estará disponible próximamente" }
            };
        }

        public async Task<bool> ValidarReimpresionAsync(string comprobanteId, string tipoComprobante)
        {
            if (string.IsNullOrEmpty(comprobanteId) || string.IsNullOrEmpty(tipoComprobante))
            {
                return false;
            }

            try
            {
                if (tipoComprobante.ToLower() == "factura")
                {
                    var factura = await _facturaBusiness.Get(comprobanteId);
                    if (factura == null)
                        return false;

                    // No se puede reimprimir si está anulada
                    if (factura.Estado == "ANULADA")
                        return false;

                    // Debe tener URL de PDF
                    if (string.IsNullOrEmpty(factura.UrlPDF))
                        return false;

                    return true;
                }

                // TODO: Agregar validación para notas cuando esté implementado
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> ObtenerUrlPdfAsync(string comprobanteId, string tipoComprobante)
        {
            if (string.IsNullOrEmpty(comprobanteId) || string.IsNullOrEmpty(tipoComprobante))
            {
                return null;
            }

            try
            {
                if (tipoComprobante.ToLower() == "factura")
                {
                    var factura = await _facturaBusiness.Get(comprobanteId);
                    return factura?.UrlPDF;
                }

                // TODO: Agregar para notas cuando esté implementado
                return null;
            }
            catch
            {
                return null;
            }
        }

        private async Task<ReimprimirFacturaResponseDTO> ReimprimirConTusFacturasAsync(Factura factura, ReimprimirFacturaRequestDTO request)
        {
            try
            {
                // Obtener credenciales de TusFacturasAPP
                var userToken = await _configuracionBusiness.GetTusFacturasUserToken();
                var apiKey = await _configuracionBusiness.GetTusFacturasApiKey();
                var apiToken = await _configuracionBusiness.GetTusFacturasApiToken();
                var baseUrl = await _configuracionBusiness.GetTusFacturasBaseUrl();

                // Validar credenciales
                if (string.IsNullOrEmpty(userToken) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiToken))
                {
                    return new ReimprimirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Credenciales de TusFacturasAPP no configuradas",
                        errores = new List<string> { "No se pueden obtener las credenciales para reimprimir la factura" }
                    };
                }

                // Preparar solicitud
                var tusFacturasRequest = new
                {
                    usertoken = userToken,
                    apikey = apiKey,
                    apitoken = apiToken,
                    reimpresion = new
                    {
                        numero_factura = factura.NumeroFactura,
                        punto_venta = factura.PuntoVenta,
                        tipo_comprobante = factura.TipoComprobante,
                        motivo = request.motivo_reimpresion ?? "Reimpresión solicitada por el usuario"
                    }
                };

                var json = JsonSerializer.Serialize(tusFacturasRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Llamar a la API
                var response = await _httpClient.PostAsync($"{baseUrl}/comprobantes/reimprimir", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var tusFacturasResponse = JsonSerializer.Deserialize<TusFacturasReimpresionResponseDTO>(responseContent);

                    if (tusFacturasResponse.exito)
                    {
                        byte[] pdfContent = null;
                        if (!string.IsNullOrEmpty(tusFacturasResponse.pdf_base64))
                        {
                            try
                            {
                                pdfContent = _facturaArchivoService.ConvertirBase64ABytes(tusFacturasResponse.pdf_base64);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error convirtiendo PDF base64: {ex.Message}");
                            }
                        }

                        var nombreArchivo = _facturaArchivoService.GenerarNombreArchivo(
                            factura.NumeroFactura, 
                            factura.TipoComprobante, 
                            "reimpresion");

                        return new ReimprimirFacturaResponseDTO
                        {
                            exito = true,
                            mensaje = "Factura reimpresa exitosamente",
                            url_pdf = tusFacturasResponse.url_pdf ?? factura.UrlPDF,
                            pdf_content = pdfContent,
                            nombre_archivo = nombreArchivo
                        };
                    }
                    else
                    {
                        return new ReimprimirFacturaResponseDTO
                        {
                            exito = false,
                            mensaje = "Error en la reimpresión con TusFacturasAPP",
                            errores = tusFacturasResponse.errores ?? new List<string> { tusFacturasResponse.error ?? "Error desconocido" }
                        };
                    }
                }
                else
                {
                    return new ReimprimirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Error en la comunicación con TusFacturasAPP",
                        errores = new List<string> { $"Error HTTP {response.StatusCode}: {responseContent}" }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReimprimirFacturaResponseDTO
                {
                    exito = false,
                    mensaje = "Error en la reimpresión con TusFacturasAPP",
                    errores = new List<string> { ex.Message }
                };
            }
        }

        private async Task<ReimprimirFacturaResponseDTO> ReimprimirConPdfExistenteAsync(Factura factura, ReimprimirFacturaRequestDTO request)
        {
            try
            {
                var pdfBytes = await _facturaArchivoService.DescargarPdfFacturaAsync(factura.UrlPDF);
                var nombreArchivo = _facturaArchivoService.GenerarNombreArchivo(
                    factura.NumeroFactura, 
                    factura.TipoComprobante, 
                    "reimpresion");

                // Actualizar auditoría
                await ActualizarAuditoriaFacturaAsync(factura, request.usuario_reimpresion);

                return new ReimprimirFacturaResponseDTO
                {
                    exito = true,
                    mensaje = "Factura reimpresa exitosamente (usando PDF existente)",
                    url_pdf = factura.UrlPDF,
                    pdf_content = pdfBytes,
                    nombre_archivo = nombreArchivo
                };
            }
            catch (Exception ex)
            {
                return new ReimprimirFacturaResponseDTO
                {
                    exito = false,
                    mensaje = "Error obteniendo el PDF de la factura",
                    errores = new List<string> { ex.Message }
                };
            }
        }

        private async Task ActualizarAuditoriaFacturaAsync(Factura factura, string usuario)
        {
            factura.FechaLog = DateTime.UtcNow;
            factura.UserLog = usuario;
            await _facturaBusiness.Update(factura);
        }
    }
}
