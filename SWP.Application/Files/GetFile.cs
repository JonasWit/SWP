using SWP.Domain.Infrastructure.Portal;
using System.IO;

namespace SWP.Application.LegalSwp.Files
{
    [TransientService]
    public class GetFile
    {
        private readonly IFileManager _fileManager;
        public GetFile(IFileManager fileManager) => _fileManager = fileManager;

        public FileStream Do(string image) => _fileManager.ImageStream(image);
    }
}
