using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.Portal
{
    public interface IFileManager
    {
        Task<string> SaveImageAsync(IFormFile image);
        Task<string> SaveImageAsync(Stream image, string name);
        Task<string> SaveImageAsync(byte[] buffer, string name);
        FileStream ImageStream(string image);
        FileStream ImageStream(string image, string imageUsage, int width, int height);
        string[] GetAllPicturesFromContent(string contentSubfolder);
        bool DeleteImage(string image);
    }
}
