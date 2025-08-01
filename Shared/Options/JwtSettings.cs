using System;

namespace Shared.Options
{
    /// <summary>
    /// Cấu hình JWT cho toàn hệ thống.
    /// Dùng để bind từ section "JwtSettings" trong appsettings.json
    /// </summary>
    public class JwtSettings
    {
        public JwtAppUserOptions AppUser { get; set; } = new();
        public JwtServiceOptions Service { get; set; } = new();
    }

    /// <summary>
    /// Cấu hình JWT dành cho người dùng cuối (AppUser)
    /// </summary>
    public class JwtAppUserOptions
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryInMinutes { get; set; }
    }

    /// <summary>
    /// Cấu hình JWT dành cho các service nội bộ (service-to-service)
    /// </summary>
    public class JwtServiceOptions
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryInMinutes { get; set; }
    }
}
