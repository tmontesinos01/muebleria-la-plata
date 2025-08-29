using Data.Interfaces;
using Entities;

namespace Data.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        public Cliente GetCliente(int id)
        {
            // Aquí iría la lógica para obtener un cliente de la base de datos
            return new Cliente { Id = id, Nombre = "Juan", Apellido = "Pérez" };
        }
    }
}