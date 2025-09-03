using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IImagenService
    {
        Task<string> SubirImagenAsync(IFormFile imagen, string nombreArchivo);
        Task EliminarImagenAsync(string fileUrl);
    }
}
