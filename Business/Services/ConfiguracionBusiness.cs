using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ConfiguracionBusiness : IConfiguracionBusiness
    {
        private readonly IRepository<Configuracion> _configuracionRepo;

        public ConfiguracionBusiness(IRepository<Configuracion> configuracionRepo)
        {
            _configuracionRepo = configuracionRepo;
        }

        public async Task<IEnumerable<Configuracion>> GetAll()
        {
            var items = await _configuracionRepo.GetAll();
            return items.Where(c => c.Activo).ToList();
        }

        public async Task<Configuracion?> Get(string id)
        {
            var configuracion = await _configuracionRepo.Get(id);
            if (configuracion == null || !configuracion.Activo) return null;
            return configuracion;
        }

        public async Task<string> Add(Configuracion configuracion)
        {
            configuracion.Activo = true;
            configuracion.FechaCreacion = DateTime.UtcNow;
            return await _configuracionRepo.Add(configuracion);
        }

        public async Task Update(Configuracion configuracion)
        {
            configuracion.FechaLog = DateTime.UtcNow;
            await _configuracionRepo.Update(configuracion);
        }

        public async Task Delete(string id)
        {
            var configuracion = await _configuracionRepo.Get(id);
            if (configuracion != null)
            {
                configuracion.Activo = false;
                configuracion.FechaLog = DateTime.UtcNow;
                await _configuracionRepo.Update(configuracion);
            }
        }

        public async Task<string> GetTusFacturasUserToken()
        {
            var configuraciones = await GetAll();
            var config = configuraciones.FirstOrDefault(c => c.Clave == "TUSFACTURAS_USER_TOKEN");
            return config?.Valor ?? "";
        }

        public async Task<string> GetTusFacturasApiKey()
        {
            var configuraciones = await GetAll();
            var config = configuraciones.FirstOrDefault(c => c.Clave == "TUSFACTURAS_API_KEY");
            return config?.Valor ?? "";
        }

        public async Task<string> GetTusFacturasApiToken()
        {
            var configuraciones = await GetAll();
            var config = configuraciones.FirstOrDefault(c => c.Clave == "TUSFACTURAS_API_TOKEN");
            return config?.Valor ?? "";
        }

        public async Task<string> GetTusFacturasBaseUrl()
        {
            var configuraciones = await GetAll();
            var config = configuraciones.FirstOrDefault(c => c.Clave == "TUSFACTURAS_BASE_URL");
            return config?.Valor ?? "https://www.tusfacturas.app/app/api/v2";
        }

        public async Task<string> GetPuntoVentaDefault()
        {
            var configuraciones = await GetAll();
            var config = configuraciones.FirstOrDefault(c => c.Clave == "PUNTO_VENTA_DEFAULT");
            return config?.Valor ?? "0001";
        }
    }
}
