using ShopSphere.API.Exceptions;
using ShopSphere.API.Interfaces;

namespace ShopSphere.API.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;


        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("No file uploaded.");
            }


            var allowedExtensions = new[]
                                  {
                                      ".jpg",
                                      ".jpeg",
                                      ".png"
                                  };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                throw new BadRequestException("Invalid file type.");
            }

            var fileName = $"{Guid.NewGuid()}{extension}";


            const long maxSize = 2 * 1024 * 1024;

            if (file.Length > maxSize)
            {
                throw new BadRequestException("Maximum file size is 2 MB.");
            }


            var folderPath = Path.Combine(_environment.WebRootPath,"uploads",folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            var filePath = Path.Combine(folderPath, fileName);
            DeleteFile(filePath);
            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"uploads/{folderName}/{fileName}";

        }

        public void DeleteFile(string filePath) 
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    return;

                var fullPath = Path.Combine(_environment.WebRootPath,filePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
    }
}
