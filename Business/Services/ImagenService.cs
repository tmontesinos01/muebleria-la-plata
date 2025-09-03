using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ImagenService : IImagenService
    {
        private readonly IFirebaseStorageService _firebaseStorageService;

        public ImagenService(IFirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<string> SubirImagenAsync(IFormFile imagen, string nombreArchivo)
        {
            return await _firebaseStorageService.UploadFileAsync(imagen, nombreArchivo);
        }

        public async Task EliminarImagenAsync(string fileUrl)
        {
            await _firebaseStorageService.DeleteFileAsync(fileUrl);
        }
    }
}
