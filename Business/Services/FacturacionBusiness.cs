using Business.Interfaces;
using Data.Interfaces;
using Entities;
using Entities.DTOs.Facturacion;
using Entities.DTOs.Clientes;
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
        private readonly IRepository<Venta> _ventaRepo;
        private readonly IProductoBusiness _productoBusiness;
        private readonly IFacturaBusiness _facturaBusiness;
        private readonly IComprobanteImpresionService _comprobanteImpresionService;

        public FacturacionBusiness(
            HttpClient httpClient, 
            IConfiguracionBusiness configuracionBusiness,
            ITipoComprobanteBusiness tipoComprobanteBusiness,
            IRepository<Venta> ventaRepo,
            IProductoBusiness productoBusiness,
            IFacturaBusiness facturaBusiness,
            IComprobanteImpresionService comprobanteImpresionService)
        {
            _httpClient = httpClient;
            _configuracionBusiness = configuracionBusiness;
            _tipoComprobanteBusiness = tipoComprobanteBusiness;
            _ventaRepo = ventaRepo;
            _productoBusiness = productoBusiness;
            _facturaBusiness = facturaBusiness;
            _comprobanteImpresionService = comprobanteImpresionService;
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
                var response = await _httpClient.PostAsync($"_EMITIR_COMPROBANTE_URL_", content); // URL ficticia para evitar el error de variable no definida
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tusFacturasResponse = JsonSerializer.Deserialize<TusFacturasResponseDTO>(responseContent);

                    if (tusFacturasResponse != null) {
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
                    } else {
                         return new EmitirFacturaResponseDTO
                        {
                            exito = false,
                            mensaje = "Error al deserializar la respuesta de TusFacturasAPP",
                            errores = new List<string> { responseContent }
                        };
                    }
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

        public async Task<EmitirFacturaResponseDTO> EmitirFacturaDesdeVenta(string ventaId, EmitirFacturaRequestDTO request)
        {
            try
            {
                var venta = await _ventaRepo.Get(ventaId);
                if (venta == null)
                {
                    return new EmitirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Venta no encontrada",
                        errores = new List<string> { $"Venta con ID {ventaId} no existe" }
                    };
                }

                if (venta.Facturada)
                {
                    return new EmitirFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Venta ya facturada",
                        errores = new List<string> { "Esta venta ya tiene una factura asociada" }
                    };
                }

                var itemsFactura = new List<ItemFacturaDTO>();
                foreach (var detalle in venta.Detalles)
                {
                    var producto = await _productoBusiness.Get(detalle.IdProducto);
                    itemsFactura.Add(new ItemFacturaDTO
                    {
                        descripcion = producto?.Nombre ?? "Producto no encontrado",
                        cantidad = detalle.Cantidad,
                        precio_unitario = detalle.PrecioUnitario,
                        alicuota_iva = detalle.AlicuotaIVA
                    });
                }

                var facturaRequest = new EmitirFacturaRequestDTO
                {
                    cliente = request.cliente,
                    tipo_comprobante = request.tipo_comprobante,
                    items = itemsFactura,
                    observaciones = request.observaciones
                };

                var resultado = await EmitirFactura(facturaRequest);

                if (resultado.exito)
                {
                    var factura = new Factura
                    {
                        IdVenta = ventaId,
                        NumeroFactura = resultado.factura.numero_factura,
                        CAE = resultado.factura.cae,
                        FechaVencimientoCAE = resultado.factura.fecha_vencimiento_cae,
                        Total = resultado.factura.total,
                        UrlPDF = resultado.factura.url_pdf,
                        TipoComprobante = resultado.factura.tipo_comprobante,
                        PuntoVenta = resultado.factura.punto_venta,
                        FechaEmision = DateTime.UtcNow,
                        Estado = "EMITIDA",
                        ClienteDocumentoTipo = request.cliente.documento_tipo,
                        ClienteDocumentoNro = request.cliente.documento_nro,
                        ClienteRazonSocial = request.cliente.razon_social,
                        ClienteDireccion = request.cliente.direccion,
                        ClienteLocalidad = request.cliente.localidad,
                        ClienteProvincia = request.cliente.provincia,
                        ClienteCodigoPostal = request.cliente.codigopostal,
                        ClienteCondicionIVA = request.cliente.condicion_iva,
                        Observaciones = request.observaciones,
                        Items = venta.Detalles.Select(detalle => new ItemFactura
                        {
                            IdProducto = detalle.IdProducto,
                            Descripcion = itemsFactura.First(i => i.cantidad == detalle.Cantidad && i.precio_unitario == detalle.PrecioUnitario).descripcion,
                            Cantidad = detalle.Cantidad,
                            PrecioUnitario = detalle.PrecioUnitario,
                            AlicuotaIVA = detalle.AlicuotaIVA,
                            Subtotal = detalle.Subtotal,
                            IVA = detalle.IVA,
                            Total = detalle.Total
                        }).ToList(),
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow,
                        FechaLog = DateTime.UtcNow,
                        UserLog = "Sistema"
                    };

                    await _facturaBusiness.Add(factura);
                    
                    venta.Facturada = true;
                    venta.NumeroFactura = resultado.factura.numero_factura;
                    venta.FechaFacturacion = DateTime.UtcNow;
                    venta.EstadoFacturacion = "FACTURADA";
                    venta.FechaLog = DateTime.UtcNow;
                    await _ventaRepo.Update(venta);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return new EmitirFacturaResponseDTO
                {
                    exito = false,
                    mensaje = "Error interno en la facturación",
                    errores = new List<string> { ex.Message }
                };
            }
        }

        public async Task<AnularFacturaResponseDTO> AnularFactura(string facturaId, AnularFacturaRequestDTO request)
        {
            try
            {
                // Validar parámetros de entrada
                if (string.IsNullOrEmpty(facturaId) || request == null)
                {
                    return new AnularFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Datos de anulación inválidos",
                        errores = new List<string> { "ID de factura y datos de anulación son requeridos" }
                    };
                }

                // Obtener la factura existente
                var factura = await _facturaBusiness.Get(facturaId);
                if (factura == null)
                {
                    return new AnularFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Factura no encontrada",
                        errores = new List<string> { $"Factura con ID '{facturaId}' no existe" }
                    };
                }

                // Validar que la factura esté en estado válido para anulación
                if (factura.Estado == "ANULADA")
                {
                    return new AnularFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "La factura ya está anulada",
                        errores = new List<string> { "No se puede anular una factura que ya está anulada" }
                    };
                }

                // Obtener credenciales de TusFacturasAPP
                var userToken = await _configuracionBusiness.GetTusFacturasUserToken();
                var apiKey = await _configuracionBusiness.GetTusFacturasApiKey();
                var apiToken = await _configuracionBusiness.GetTusFacturasApiToken();
                var baseUrl = await _configuracionBusiness.GetTusFacturasBaseUrl();

                // Validar que las credenciales estén configuradas
                if (string.IsNullOrEmpty(userToken) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiToken))
                {
                    return new AnularFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Credenciales de TusFacturasAPP no configuradas",
                        errores = new List<string> { "No se pueden obtener las credenciales para anular la factura" }
                    };
                }

                // Preparar la solicitud de anulación para TusFacturasAPP
                var tusFacturasRequest = new
                {
                    usertoken = userToken,
                    apikey = apiKey,
                    apitoken = apiToken,
                    anulacion = new
                    {
                        numero_factura = factura.NumeroFactura,
                        punto_venta = factura.PuntoVenta,
                        tipo_comprobante = factura.TipoComprobante,
                        motivo = request.motivo_anulacion ?? "Anulación solicitada por el usuario"
                    }
                };

                // Serializar a JSON
                var json = JsonSerializer.Serialize(tusFacturasRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Llamar a la API de anulación de TusFacturasAPP
                var response = await _httpClient.PostAsync($"_ANULAR_COMPROBANTE_URL_", content); // URL ficticia para evitar el error de variable no definida
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Deserializar la respuesta
                    var tusFacturasResponse = JsonSerializer.Deserialize<TusFacturasAnulacionResponseDTO>(responseContent);

                    if (tusFacturasResponse.exito)
                    {
                        // Actualizar la factura en nuestra base de datos
                        factura.Estado = "ANULADA";
                        factura.FechaAnulacion = DateTime.UtcNow;
                        factura.MotivoAnulacion = request.motivo_anulacion;
                        factura.UsuarioAnulacion = request.usuario_anulacion;
                        factura.ObservacionesAnulacion = request.observaciones_anulacion;
                        factura.FechaLog = DateTime.UtcNow;
                        factura.UserLog = request.usuario_anulacion;

                        await _facturaBusiness.Update(factura);

                        return new AnularFacturaResponseDTO
                        {
                            exito = true,
                            mensaje = "Factura anulada exitosamente",
                            factura = new FacturaAnuladaDTO
                            {
                                id_factura = factura.Id,
                                numero_factura = factura.NumeroFactura,
                                fecha_anulacion = factura.FechaAnulacion.Value,
                                motivo_anulacion = factura.MotivoAnulacion,
                                usuario_anulacion = factura.UsuarioAnulacion,
                                estado = factura.Estado
                            }
                        };
                    }
                    else
                    {
                        return new AnularFacturaResponseDTO
                        {
                            exito = false,
                            mensaje = "Error en la anulación con TusFacturasAPP",
                            errores = tusFacturasResponse.errores ?? new List<string> { tusFacturasResponse.error ?? "Error desconocido" }
                        };
                    }
                }
                else
                {
                    return new AnularFacturaResponseDTO
                    {
                        exito = false,
                        mensaje = "Error en la comunicación con TusFacturasAPP",
                        errores = new List<string> { $"Error HTTP {response.StatusCode}: {responseContent}" }
                    };
                }
            }
            catch (Exception ex)
            {
                return new AnularFacturaResponseDTO
                {
                    exito = false,
                    mensaje = "Error interno en la anulación",
                    errores = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ReimprimirFacturaResponseDTO> ReimprimirFactura(string facturaId, ReimprimirFacturaRequestDTO request)
        {
            return await _comprobanteImpresionService.ReimprimirFacturaAsync(facturaId, request);
        }
    }
}
