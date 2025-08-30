using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business
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

        public Task<string> CreateCategoria(Categoria categoria)
        {
            return _categoriaRepositorio.Add(categoria);
        }

        public Task UpdateCategoria(Categoria categoria)
        {
            return _categoriaRepositorio.Update(categoria);
        }

        public Task DeleteCategoria(string id)
        {
            return _categoriaRepositorio.Delete(id);
        }
    }
}
