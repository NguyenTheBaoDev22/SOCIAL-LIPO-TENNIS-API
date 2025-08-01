using Applications.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Implementations
{
    public class LarkAuthService : ILarkAuthService
    {
        private readonly ILogger<LarkAuthService> _logger;
        private readonly ILarkSuiteConfig _config;

        public LarkAuthService(ILogger<LarkAuthService> logger, ILarkSuiteConfig config)
        {
            _logger = logger;
            _config = config;

            // Log cấu hình để dễ dàng debug
            _logger.LogInformation($"LarkAuthService initialized with AppId: {_config.AppId}, RedirectUri: {_config.RedirectUri}, AuthBaseUrl: {_config.AuthBaseUrl}, Scope: {_config.Scope}");
        }

        /// <inheritdoc />
        public string GenerateAuthorizationUrl(string state)
        {
            // Kiểm tra các giá trị cấu hình cần thiết
            if (string.IsNullOrEmpty(_config.AuthBaseUrl) ||
                string.IsNullOrEmpty(_config.AppId) ||
                string.IsNullOrEmpty(_config.RedirectUri) ||
                string.IsNullOrEmpty(_config.Scope))
            {
                _logger.LogError("LarkSuite authentication configuration (AuthBaseUrl, AppId, RedirectUri, Scope) is incomplete.");
                throw new InvalidOperationException("LarkSuite authentication configuration is missing. Please check your appsettings.json.");
            }

            // Xây dựng URL ủy quyền với các tham số đã mã hóa
            // Sử dụng AuthBaseUrl, AppId, RedirectUri, Scope từ cấu hình
            string authorizationUrl = $"{_config.AuthBaseUrl}?app_id={_config.AppId}&redirect_uri={Uri.EscapeDataString(_config.RedirectUri)}&scope={_config.Scope}&state={state}";

            _logger.LogInformation($"Generated Lark Authorization URL: {authorizationUrl}");
            return authorizationUrl;
        }
    }
}
