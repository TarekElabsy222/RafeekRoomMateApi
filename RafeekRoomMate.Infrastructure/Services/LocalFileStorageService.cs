using Microsoft.AspNetCore.Hosting;
using RafeekRoomMate.Application.Common.Interfaces;

namespace RafeekRoomMate.Infrastructure.Services
{
    public sealed class LocalFileStorageService : IFileStorageService
    {
        #region fields
        private readonly IWebHostEnvironment _environment;
        #endregion

        #region constructor
        public LocalFileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        #endregion

        #region handle functions
        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, string folder, CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(fileName);

            var generatedFileName = $"{Guid.NewGuid():N}{extension}";

            var relativeFolder = folder.Replace("/", Path.DirectorySeparatorChar.ToString());

            var uploadsRoot = Path.Combine(_environment.WebRootPath, "uploads");

            var directory = Path.Combine(uploadsRoot, relativeFolder);

            Directory.CreateDirectory(directory);

            var filePath = Path.Combine(directory, generatedFileName);

            await using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 64 * 1024, useAsync: true);

            await stream.CopyToAsync(fileStream, cancellationToken);

            return Path.Combine(folder, generatedFileName).Replace("\\", "/");

        }

        public Task DeleteAsync(string fileKey, CancellationToken cancellationToken = default)
        {
            var filePath =Path.Combine(_environment.WebRootPath,"uploads", fileKey.Replace("/",Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }

        #endregion
}

