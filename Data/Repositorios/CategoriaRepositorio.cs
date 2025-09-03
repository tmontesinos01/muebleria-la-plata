using Data.Interfaces;
using Entities;

namespace Data.Repositorios
{
    public class CategoriaRepositorio : RepositorioBase<Categoria>, ICategoriaRepositorio
    {
        public CategoriaRepositorio() : base("categorias")
        {
        }
    }
}
