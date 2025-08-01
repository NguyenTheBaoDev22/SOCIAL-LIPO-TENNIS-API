namespace Shared.DTOs
{
    public class TokenResult
    {
        public string AccessToken { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; }
    }
}
