using Business.Interfaces;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Services
{
    public class FacturacionBusiness : IFacturacionBusiness
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguracionBusiness _configuracionBusiness;
        private readonly ITipoComprobanteBusiness _tipoComprobanteBusiness;

        public FacturacionBusiness(
            HttpClient httpClient, 
            IConfiguracionBusiness configuracionBusiness,
            ITipoComprobanteBusiness tipoComprobanteBusiness)
        {
            _httpClient = httpClient;
            _configuracionBusiness = configuracionBusiness;
            _tipoComprobanteBusiness = tipoComprobanteBusiness;
        }

        public async Task<EmitirFacturaResponseDTO> EmitirFactura(EmitirFacturaRequestDTO request)
        {
            try
            {
                // Validar parámetros de entrada
                if (request == null || request.cliente == null || request.items == null || !request.items.Any())
                {
                    return new EmitirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Datos de factura inválidos",
                        errores = new List<string> { "Cliente e items son requeridos" }
                    };
                }

                // Obtener credenciales desde configuración
                var userToken = await _configuracionBusiness.GetTusFacturasUserToken();
                var apiKey = await _configuracionBusiness.GetTusFacturasApiKey();
                var apiToken = await _configuracionBusiness.GetTusFacturasApiToken();
                var baseUrl = await _configuracionBusiness.GetTusFacturasBaseUrl();
                var puntoVenta = await _configuracionBusiness.GetPuntoVentaDefault();

                // Validar que las credenciales estén configuradas
                if (string.IsNullOrEmpty(userToken) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiToken))
                {
                    return new EmitirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Credenciales de TusFacturasAPP no configuradas",
                        errores = new List<string> { "Configurar credenciales en la tabla Configuracion" }
                    };
                }

                // Validar tipo de comprobante
                var tipoComprobante = await _tipoComprobanteBusiness.Get(request.tipo_comprobante);
                if (tipoComprobante == null)
                {
                    return new EmitirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Tipo de comprobante inválido",
                        errores = new List<string> { $"Tipo de comprobante '{request.tipo_comprobante}' no encontrado" }
                    };
                }

                // Preparar la solicitud para TusFacturasAPP
                var tusFacturasRequest = new
                {
                    usertoken = userToken,
                    apikey = apiKey,
                    apitoken = apiToken,
                    comprobante = new
                    {
                        tipo = request.tipo_comprobante,
                        punto_venta = puntoVenta,
                        cliente = request.cliente,
                        items = request.items.Select(item => new
                        {
                            descripcion = item.descripcion,
                            cantidad = item.cantidad,
                            precio_unitario = item.precio_unitario,
                            alicuota_iva = item.alicuota_iva
                        }),
                        observaciones = request.observaciones ?? ""
                    }
                };

                // Serializar a JSON
                var json = JsonSerializer.Serialize(tusFacturasRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la llamada a TusFacturasAPP
                var response = await _httpClient.PostAsync($"{baseUrl}/comprobantes/emitir", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tusFacturasResponse = JsonSerializer.Deserialize<TusFacturasResponseDTO>(responseContent);
                    
                    // Mapear la respuesta exitosa
                    return new EmitirFacturaResponseDTO
                    {
                        exito = true,
                        mensaje = "Factura emitida correctamente",
                        factura = new FacturaEmitidaDTO
                        {
                            numero_factura = tusFacturasResponse.numero_factura,
                            cae = tusFacturasResponse.cae,
                            fecha_vencimiento_cae = tusFacturasResponse.fecha_vencimiento_cae,
                            total = tusFacturasResponse.total,
                            url_pdf = tusFacturasResponse.url_pdf,
                            tipo_comprobante = request.tipo_comprobante,
                            punto_venta = puntoVenta
                        },
                        errores = new List<string>()
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new EmitirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = $"Error en la emisión: {response.StatusCode}",
                        errores = new List<string> { errorContent }
                    };
                }
            }
            catch (Exception ex)
            {
                return new EmitirFacturaResponseDTO
                {
                    exito = false,
                    mensaje = "Error interno en la emisión",
                    errores = new List<string> { ex.Message }
                };
            }
        }
    }
}
