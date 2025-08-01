using Applications.Features.LarksuiteIntegrations.DTOs;
using Applications.Services.Implementations;
using Applications.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using Shared.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LarksuiteController : ControllerBase
    {
        private readonly ILarkEmailService _larkEmailService;
        private readonly ILarkTokenService _larkTokenService;
        private readonly ILogger<LarksuiteController> _logger;
        private readonly ICurrentUserService _currentUser;
        private readonly ILarkAuthService _larkAuthService; // Service để lấy và làm mới token

        public LarksuiteController(
            ILarkEmailService larkEmailService,
            ILarkTokenService larkTokenService,
            ILogger<LarksuiteController> logger,
            ICurrentUserService currentUser,
            ILarkAuthService larkAuthService)
        {
            _larkEmailService = larkEmailService;
            _larkTokenService = larkTokenService;
            _logger = logger;
            _currentUser = currentUser;
            _larkAuthService = larkAuthService;
        }
        /// <summary>
        /// Endpoint để lấy URL ủy quyền Larksuite.
        /// Người dùng sẽ được chuyển hướng đến URL này để cấp quyền.
        /// </summary>
        [HttpGet("connect-lark")]
        public IActionResult ConnectLark()
        {
            // Tạo state để chống tấn công CSRF
            string state = Guid.NewGuid().ToString();
            // TODO: Lưu state này vào session hoặc một nơi an toàn để kiểm tra sau khi nhận callback
            _logger.LogInformation($"Generating Lark authorization URL with state: {state}");

            // Gọi service để tạo URL ủy quyền
            string authorizationUrl = _larkAuthService.GenerateAuthorizationUrl(state);

            // Trả về URL để frontend có thể chuyển hướng người dùng
            return Ok(authorizationUrl);
        }

        [HttpPost("get-lark-access-token")]
        public async Task<IActionResult> GetLarkAccessToken([FromForm] string code, [FromForm] string state)
        {
            _logger.LogInformation($"Request at /get-lark-access-token with code: {code}, state: {state}");

            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Missing auth code.");
                return BadRequest("Authorization code is required.");
            }

            var tokenResult = await _larkTokenService.GetAccessTokenAsync(code, state);
            if (tokenResult != null)
                return Ok(tokenResult);

            return StatusCode(500, "Không thể lấy Access Token.");
        }
        /// <summary>
        /// Gửi email qua Lark Suite
        /// </summary>
        [HttpPost("send-email")]
        public async Task<ActionResult<BaseResponse<string>>> SendEmailAsync(
            [FromBody] LarkEmailDto dto,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("📤 Gửi email qua LarkSuite tới {To} | TraceId: {TraceId}", dto.RecipientEmail, _currentUser.TraceId);

            var tokenResult = await _larkTokenService.GetValidTokenAsync();
            if (tokenResult==null)
            {
                return BadRequest(BaseResponse<string>.Error("E-LARK-TOKEN", "Không thể lấy access token từ Lark"));
            }

            var result = await _larkEmailService.SendEmailAsync(dto, tokenResult.AccessToken, cancellationToken);
            return Ok(result);

        }

        // ⏳ Các API khác như get-user-info, refresh-token, send-doc, send-message... sẽ thêm tại đây
    }
}

