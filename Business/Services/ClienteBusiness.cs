using Business.Interfaces;
using Data.Interfaces;
using Entities;
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
    public class ClienteBusiness : IClienteBusiness
    {
        private readonly IRepository<Cliente> _clienteRepo;
        private readonly HttpClient _httpClient;
        private readonly IConfiguracionBusiness _configuracionBusiness;

        public ClienteBusiness(IRepository<Cliente> clienteRepo, HttpClient httpClient, IConfiguracionBusiness configuracionBusiness)
        {
            _clienteRepo = clienteRepo;
            _httpClient = httpClient;
            _configuracionBusiness = configuracionBusiness;
        }

        public async Task<IEnumerable<Cliente>> GetAll()
        {
            var items = await _clienteRepo.GetAll();
            return items.Where(c => c.Activo).ToList();
        }

        public async Task<Cliente?> Get(string id)
        {
            var cliente = await _clienteRepo.Get(id);
            if (cliente == null || !cliente.Activo) return null;
            return cliente;
        }

        public async Task<string> Add(Cliente cliente)
        {
            cliente.Activo = true;
            cliente.FechaCreacion = DateTime.UtcNow;
            return await _clienteRepo.Add(cliente);
        }

        public async Task Update(Cliente cliente)
        {
            cliente.FechaLog = DateTime.UtcNow;
            await _clienteRepo.Update(cliente);
        }

        public async Task Delete(string id)
        {
            var cliente = await _clienteRepo.Get(id);
            if (cliente != null)
            {
                cliente.Activo = false;
                cliente.FechaLog = DateTime.UtcNow;
                await _clienteRepo.Update(cliente);
            }
        }

        public async Task<ClienteFacturacionDTO> ValidarClienteAFIP(ValidarClienteRequestDTO request)
        {
            try
            {
                // Validar parámetros de entrada
                if (string.IsNullOrEmpty(request.documento_nro) || string.IsNullOrEmpty(request.documento_tipo))
                {
                    return new ClienteFacturacionDTO
                    {
                        es_valido = false,
                        errores = new List<string> { "Documento y tipo son requeridos" }
                    };
                }

                // Obtener credenciales desde configuración
                var userToken = await _configuracionBusiness.GetTusFacturasUserToken();
                var apiKey = await _configuracionBusiness.GetTusFacturasApiKey();
                var apiToken = await _configuracionBusiness.GetTusFacturasApiToken();
                var baseUrl = await _configuracionBusiness.GetTusFacturasBaseUrl();

                // Validar que las credenciales estén configuradas
                if (string.IsNullOrEmpty(userToken) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiToken))
                {
                    return new ClienteFacturacionDTO
                    {
                        es_valido = false,
                        errores = new List<string> { "Credenciales de TusFacturasAPP no configuradas" }
                    };
                }

                // Preparar la solicitud para TusFacturasAPP
                var tusFacturasRequest = new
                {
                    usertoken = userToken,
                    apikey = apiKey,
                    apitoken = apiToken,
                    cliente = new
                    {
                        documento_nro = request.documento_nro,
                        documento_tipo = request.documento_tipo
                    }
                };

                // Serializar a JSON
                var json = JsonSerializer.Serialize(tusFacturasRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la llamada a TusFacturasAPP
                var response = await _httpClient.PostAsync($"{baseUrl}/clientes/afip-info", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var afipResponse = JsonSerializer.Deserialize<ValidarClienteResponseDTO>(responseContent);
                    
                    // Mapear la respuesta de AFIP a los datos necesarios para facturación
                    return MapearClienteParaFacturacion(afipResponse, request);
                }
                else
                {
                    return new ClienteFacturacionDTO
                    {
                        es_valido = false,
                        errores = new List<string> { $"Error en la consulta: {response.StatusCode}" }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ClienteFacturacionDTO
                {
                    es_valido = false,
                    errores = new List<string> { $"Error interno: {ex.Message}" }
                };
            }
        }

        private ClienteFacturacionDTO MapearClienteParaFacturacion(ValidarClienteResponseDTO afipResponse, ValidarClienteRequestDTO request)
        {
            // Verificar si hay errores en la respuesta de AFIP
            if (afipResponse.error == "S" || (afipResponse.errores != null && afipResponse.errores.Any()))
            {
                return new ClienteFacturacionDTO
                {
                    es_valido = false,
                    errores = afipResponse.errores ?? new List<string> { "Error en la consulta AFIP" }
                };
            }

            // Mapear condición impositiva a código AFIP
            var condicionIva = MapearCondicionImpositiva(afipResponse.condicion_impositiva);

            return new ClienteFacturacionDTO
            {
                cliente = new ClienteAFIPDTO
                {
                    documento_tipo = request.documento_tipo,
                    documento_nro = request.documento_nro,
                    razon_social = afipResponse.razon_social,
                    direccion = afipResponse.direccion,
                    localidad = afipResponse.localidad,
                    provincia = afipResponse.provincia,
                    codigopostal = afipResponse.codigopostal,
                    condicion_iva = condicionIva
                },
                es_valido = afipResponse.estado == "ACTIVO",
                errores = new List<string>()
            };
        }

        private string MapearCondicionImpositiva(string condicionImpositiva)
        {
            // Mapear las condiciones impositivas de AFIP a códigos para facturación
            return condicionImpositiva?.ToUpper() switch
            {
                "RESPONSABLE INSCRIPTO" => "1", // Responsable Inscripto
                "MONOTRIBUTO" => "6", // Monotributo
                "EXENTO" => "4", // Exento
                "NO RESPONSABLE" => "2", // No Responsable
                "CONSUMIDOR FINAL" => "5", // Consumidor Final
                _ => "5" // Por defecto Consumidor Final
            };
        }
    }
}
