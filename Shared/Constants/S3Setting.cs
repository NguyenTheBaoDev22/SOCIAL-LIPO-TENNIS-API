namespace Shared.Constants
{
    public class S3Setting
    {
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string BucketName { get; set; } = null!;
        public string Region { get; set; } = "ap-southeast-1";
        public bool AllowAcl { get; set; } = false;
    }
}
