using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ITipoComprobanteBusiness
    {
        Task<IEnumerable<TipoComprobante>> GetAll();
        Task<TipoComprobante?> Get(string id);
        Task<string> Add(TipoComprobante entity);
        Task Update(TipoComprobante entity);
        Task Delete(string id);
    }
}
