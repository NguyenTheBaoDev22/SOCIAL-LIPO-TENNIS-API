using Applications.Features.AppAccounts.Commands;
using Applications.Features.Notifications.Commands;
using Applications.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Authorization;
using Shared.Results;
using Zenshop.WebAPI.Filters;

namespace API.Controllers
{
    [ApiController]
    [Route("api/app-auth")]
    public class AppAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginAppUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Thiết lập mật khẩu lần đầu (sau khi admin duyệt user)
        /// </summary>
        [HttpPost("setup-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetupPassword([FromBody] SetupPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Gửi OTP đặt lại mật khẩu (qua số điện thoại)
        /// </summary>
        [HttpPost("forgot-password/request-otp")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestForgotPasswordOtp([FromBody] RequestForgotPasswordOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("forgot-password/verify-otp")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyForgotPasswordOtp([FromBody] VerifyOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("admin/reset-otp-counter")]
        [Authorize]
        [AuthorizePermission(PermissionCode.ApproveMerchant)]
        public async Task<IActionResult> ResetOtpCounter([FromBody] ResetOtpRequestCounterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("change-password")]
        [Authorize] // phải đăng nhập mới được đổi
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
