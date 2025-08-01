using Core.Enumerables;

namespace Applications.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, FileVisibility visibility, CancellationToken cancellationToken = default);
        Task<Stream> GetFileAsync(string fileKey, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string fileKey, CancellationToken cancellationToken = default);
        string GetPreSignedUrl(string fileKey, TimeSpan expiresIn);
        string GenerateSignedUrl(string fileKey, TimeSpan expiresIn); // 👈 new
    }
}
