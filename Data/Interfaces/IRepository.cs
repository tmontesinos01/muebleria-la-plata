using Entities.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(string id);
        Task<string> Add(T entity);
        Task Update(T entity);
        Task Delete(string id);
    }
}
