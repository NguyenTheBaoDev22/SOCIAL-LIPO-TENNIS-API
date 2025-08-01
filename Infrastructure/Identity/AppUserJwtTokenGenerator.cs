using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using Shared.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    /// <summary>
    /// Sinh JWT Token cho người dùng ứng dụng (AppUser)
    /// </summary>
    public class AppUserJwtTokenGenerator : IAppUserJwtTokenGenerator
    {
        private readonly JwtAppUserOptions _options;
        private readonly ILogger<AppUserJwtTokenGenerator> _logger;

        public AppUserJwtTokenGenerator(IOptions<JwtSettings> jwtSettings, ILogger<AppUserJwtTokenGenerator> logger)
        {
            _options = jwtSettings.Value.AppUser;
            _logger = logger;
        }

        public string GenerateToken(AppUserTokenPayload payload, Guid? tenantId, Guid? merchantId, Guid? branchId)
        {
            try
            {
                // Log thông tin payload (ẩn thông tin nhạy cảm nếu cần)
                _logger.LogInformation("Generating JWT for UserId: {UserId}, Email: {Email}, TenantId: {Tenant}, MerchantId: {Merchant}, BranchId: {Branch}",
                    payload.UserId, payload.Email, tenantId, merchantId, branchId);

                // Validate trước khi tạo claim
                if (payload.UserId == Guid.Empty)
                    throw new ArgumentException("UserId không hợp lệ khi tạo JWT");

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, payload.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, payload.Email ?? string.Empty),
                    new Claim("phone_number", payload.PhoneNumber ?? string.Empty),
                };

                if (tenantId != null)
                    claims.Add(new Claim("tenant_id", tenantId.ToString()!));
                if (merchantId != null)
                    claims.Add(new Claim("merchant_id", merchantId.ToString()!));
                if (branchId != null)
                    claims.Add(new Claim("branch_id", branchId.ToString()!));

                if (payload.Roles != null)
                    claims.AddRange(payload.Roles.Select(role => new Claim("role", role)));
                if (payload.Permissions != null)
                    claims.AddRange(payload.Permissions.Select(p => new Claim("permission", p)));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.UtcNow.AddMinutes(_options.ExpiryInMinutes);

                var token = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                _logger.LogInformation("Token created successfully for user {UserId}", payload.UserId);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo JWT cho UserId: {UserId}", payload?.UserId);
                throw; // Để middleware xử lý tiếp
            }
        }

        /// <summary>
        /// Sinh token cho trường hợp không truyền Tenant/Merchant/Branch
        /// </summary>
        public string GenerateToken(AppUserTokenPayload payload)
        {
            return GenerateToken(payload, tenantId: null, merchantId: null, branchId: null);
        }

        /// <summary>
        /// Dùng để sinh token cho thiết lập mật khẩu lần đầu
        /// </summary>
        public string GeneratePasswordSetupToken(Guid userId, string email, string phoneNumber)
        {
            var payload = new AppUserTokenPayload
            {
                UserId = userId,
                Email = email,
                PhoneNumber = phoneNumber,
                Roles = null,
                Permissions = null,
                TenantId = null,
                MerchantId = null,
                MerchantBranchId = null
            };

            return GenerateToken(payload);
        }
    }
}
