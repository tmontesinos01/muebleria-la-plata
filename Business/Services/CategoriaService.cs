using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public CategoriaService(ICategoriaRepositorio categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }

        public Task<IEnumerable<Categoria>> GetAllCategorias()
        {
            return _categoriaRepositorio.GetAll();
        }

        public Task<Categoria> GetCategoriaById(string id)
        {
            return _categoriaRepositorio.Get(id);
        }

        public async Task<string> CreateCategoria(Categoria categoria)
        {
            var newCategoria = await _categoriaRepositorio.Add(categoria);
            return newCategoria.Id;
        }

        public Task UpdateCategoria(Categoria categoria)
        {
            return _categoriaRepositorio.Update(categoria.Id, categoria);
        }

        public Task DeleteCategoria(string id)
        {
            return _categoriaRepositorio.Delete(id);
        }
    }
}
