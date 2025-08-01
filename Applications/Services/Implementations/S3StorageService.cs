using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Applications.Services.Interfaces;
using Core.Enumerables;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace Applications.Services.Implementations
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3Setting _settings;

        public S3StorageService(IOptions<S3Setting> settings)
        {
            _settings = settings.Value;
            _s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, RegionEndpoint.GetBySystemName(_settings.Region));
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, FileVisibility visibility, CancellationToken cancellationToken)
        {
            try
            {
                string folder = visibility switch
                {
                    FileVisibility.Public => "public/",
                    FileVisibility.Private => "private/",
                    FileVisibility.Temp => "temp/",
                    _ => ""
                };

                var fullKey = $"{folder}{fileName}";

                var request = new PutObjectRequest
                {
                    BucketName = _settings.BucketName,
                    Key = fullKey,
                    InputStream = fileStream,
                    ContentType = contentType
                };

                // Chỉ gán ACL nếu cấu hình cho phép và yêu cầu public
                if (_settings.AllowAcl && visibility == FileVisibility.Public)
                {
                    request.CannedACL = S3CannedACL.PublicRead;
                }

                await _s3Client.PutObjectAsync(request, cancellationToken);

                return visibility == FileVisibility.Public
                    ? $"https://{_settings.BucketName}.s3.{_settings.Region}.amazonaws.com/{fullKey}"
                    : fullKey;
            }
            catch (AmazonS3Exception ex)
            {
                // Ghi log chi tiết
                Console.WriteLine($"S3 Error: {ex.Message}");

                throw new ApplicationException($"Lỗi S3: {ex.Message}"); // sẽ được bắt bởi middleware của bạn
            }
        }



        public async Task<Stream> GetFileAsync(string fileKey, CancellationToken cancellationToken = default)
        {
            var response = await _s3Client.GetObjectAsync(_settings.BucketName, fileKey, cancellationToken);
            return response.ResponseStream;
        }

        public async Task DeleteFileAsync(string fileKey, CancellationToken cancellationToken = default)
        {
            await _s3Client.DeleteObjectAsync(_settings.BucketName, fileKey, cancellationToken);
        }

        public string GetPreSignedUrl(string fileKey, TimeSpan expiresIn)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.Add(expiresIn)
            };

            return _s3Client.GetPreSignedURL(request);
        }
        public string GenerateSignedUrl(string fileKey, TimeSpan expiresIn)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.Add(expiresIn),
                Verb = HttpVerb.GET
            };

            return _s3Client.GetPreSignedURL(request);
        }
    }
}
