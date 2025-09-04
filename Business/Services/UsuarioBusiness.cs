using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UsuarioBusiness : IUsuarioBusiness
    {
        private readonly IRepository<Usuario> _repository;

        public UsuarioBusiness(IRepository<Usuario> repository)
        {
            _repository = repository;
        }

        public async Task<string> Add(Usuario entity)
        {
            return await _repository.Add(entity);
        }

        public async Task Delete(string id)
        {
            await _repository.Delete(id);
        }

        public async Task<Usuario> Get(string id)
        {
            return await _repository.Get(id);
        }

        public async Task<IEnumerable<Usuario>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task Update(Usuario entity)
        {
            await _repository.Update(entity);
        }
    }
}
