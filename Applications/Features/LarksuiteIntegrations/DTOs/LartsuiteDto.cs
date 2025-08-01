using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.LarksuiteIntegrations.DTOs
{
    public class LarkEmailDto
    {
        public string FromEmail { get; set; } = default!;
        public string RecipientEmail { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;
    }
    public class LarkTokenDto
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public int ExpiresIn { get; set; }
        public int RefreshTokenExpiresIn { get; set; }
        public string TokenType { get; set; } = default!;
    }
    public class LarkTokenCache
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public int ExpiresIn { get; set; }
        public int RefreshTokenExpiresIn { get; set; }
        public string TokenType { get; set; } = default!;
        public DateTime AccessTokenExpiresAt { get; set; }
        public DateTime RefreshTokenRefreshTokenExpiresAt { get; set; }
    }
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = default!;
    }
}
