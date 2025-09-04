using Entities;
using Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUsuarioBusiness
    {
        Task<IEnumerable<Usuario>> GetAll();
        Task<Usuario> Get(string id);
        Task<string> Add(Usuario entity);
        Task Update(Usuario entity);
        Task Delete(string id);
        Task<AuthResponseDTO> AutenticarUsuario(AuthRequestDTO authRequest);
    }
}
