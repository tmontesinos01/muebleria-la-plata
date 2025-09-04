using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUnidadMedidaBusiness
    {
        Task<UnidadMedida?> Get(string id);
        Task<IEnumerable<UnidadMedida>> GetAll();
        Task<string> Add(UnidadMedida entity);
        Task Update(UnidadMedida entity);
        Task Delete(string id);
    }
}
