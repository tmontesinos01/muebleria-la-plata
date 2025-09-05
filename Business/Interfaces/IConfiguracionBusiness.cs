using Entities;
using Data.Interfaces;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IConfiguracionBusiness : IRepository<Configuracion>
    {
        Task<string> GetTusFacturasUserToken();
        Task<string> GetTusFacturasApiKey();
        Task<string> GetTusFacturasApiToken();
        Task<string> GetTusFacturasBaseUrl();
        Task<string> GetPuntoVentaDefault();
        Task<Configuracion> GetByCodigo(string codigo);
        Task<Configuracion> GetByClave(string clave);
    }
}
