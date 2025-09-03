using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Interfaces;

namespace Data.Interfaces
{
    public interface IRepositorio<T> where T : IEntity
    {
        Task<T> Get(string id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task Update(string id, T entity);
        Task Delete(string id);
    }
}
