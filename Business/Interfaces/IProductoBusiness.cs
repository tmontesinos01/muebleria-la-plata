using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductoBusiness
    {
        Task<IEnumerable<Producto>> GetAll();
        Task<Producto?> Get(string id);
        Task<string> Add(Producto producto);
        Task Update(Producto producto);
        Task Delete(string id);
    }
}
