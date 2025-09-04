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
    }
}
