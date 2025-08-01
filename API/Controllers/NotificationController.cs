using Applications.DTOs;
using Applications.Features.Notifications.Commands;
using Core.Enumerables;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{RoleEnum.AdminDashboard}, {RoleEnum.MobileShopManager}")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("sms/account-registration/otp")]
        public async Task<IActionResult> SendAccountRegistrationSMSOtpAsync([FromBody] SmsOTPRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Phone) || string.IsNullOrWhiteSpace(request.OTP))
                return BadRequest("Thiếu số điện thoại hoặc nội dung tin nhắn.");

            var dto = new AccountCreationSMSOtp
            {
                PhoneNumber = request.Phone,
                OtpCode = request.OTP,
                // Url = "https://zenpay.vn", // hoặc lấy từ config
                ExpirationInMinutes = request.ExpirationInMinutes
            };

            var response = await _mediator.Send(new SendAccountRegistrationSmsOtpCommand { SmsOtp = dto });

            if (response.Code == "00")
                return Ok(response);
            else
                return BadRequest(response);
        }
        [HttpPost("sms/account-registration/confirmation")]
        public async Task<IActionResult> SendAccountCreationSMSConfirmationAsync([FromBody] SMSAccountCreationConfirmationRequestDto request)
        {
            // Chuyển đổi từ request sang DTO gửi SMS
            var dto = new AccountCreationSMSConfirmation
            {
                ShopAddress = request.ShopAddress,
                ShopCode = request.ShopCode, // Đảm bảo rằng OTP được truyền vào đúng field
                ShopOwnerName = request.ShopOwnerName,
                ShopOwnerPhone = request.ShopOwnerPhone,
                ShopName = request.ShopName

            };

            // Gọi command gửi SMS xác nhận tài khoản
            var response = await _mediator.Send(new SendAccountCreationSMSConfirmationCommand { SmsConfirmation = dto });

            // Kiểm tra phản hồi từ dịch vụ gửi SMS
            if (response.Code == "00")
                return Ok(response); // Thành công
            else
                return BadRequest(response); // Lỗi
        }
        [HttpPost("zns/account-registration/otp")]
        public async Task<IActionResult> SendAccountRegistrationZNSOtpAsync([FromBody] SmsOTPRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Phone) || string.IsNullOrWhiteSpace(request.OTP))
                return BadRequest("Thiếu số điện thoại hoặc nội dung tin nhắn.");

            var dto = new AccountCreationSMSOtp
            {
                PhoneNumber = request.Phone,
                OtpCode = request.OTP,
                // Url = "https://zenpay.vn", // hoặc lấy từ config
                ExpirationInMinutes = request.ExpirationInMinutes
            };

            var response = await _mediator.Send(new SendAccountRegistrationSmsOtpCommand { SmsOtp = dto });

            if (response.Code == "00")
                return Ok(response);
            else
                return BadRequest(response);
        }
        [HttpPost("zns/account-registration/confirmation")]
        public async Task<IActionResult> SendAccountCreationZNSConfirmationAsync([FromBody] SMSAccountCreationConfirmationRequestDto request)
        {
            // Chuyển đổi từ request sang DTO gửi SMS
            var dto = new AccountCreationSMSConfirmation
            {
                ShopAddress = request.ShopAddress,
                ShopCode = request.ShopCode, // Đảm bảo rằng OTP được truyền vào đúng field
                ShopOwnerName = request.ShopOwnerName,
                ShopOwnerPhone = request.ShopOwnerPhone
            };

            // Gọi command gửi SMS xác nhận tài khoản
            var response = await _mediator.Send(new SendAccountCreationSMSConfirmationCommand { SmsConfirmation = dto });

            // Kiểm tra phản hồi từ dịch vụ gửi SMS
            if (response.Code == "00")
                return Ok(response); // Thành công
            else
                return BadRequest(response); // Lỗi
        }
      

    }
}
