using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string destinationPath);
        Task<byte[]> DownloadFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
    }
}
