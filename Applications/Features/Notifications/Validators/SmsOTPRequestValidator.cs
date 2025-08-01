using Applications.DTOs;
using FluentValidation;

namespace Applications.Features.Notifications.Validators
{
    public class SmsOTPRequestValidator : AbstractValidator<SmsOTPRequestDto>
    {
        public SmsOTPRequestValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Số điện thoại là bắt buộc")
                .Matches(@"^\d+$").WithMessage("Số điện thoại chỉ được chứa số");

            RuleFor(x => x.OTP)
                .NotEmpty().WithMessage("Mã OTP là bắt buộc")
                .Matches(@"^\d{4}$").WithMessage("OTP phải đúng 4 chữ số");

            RuleFor(x => x.ExpirationInMinutes)
                .NotEmpty().WithMessage("Thời gian hiệu lực là bắt buộc")
                .Matches(@"^\d+$").WithMessage("Thời gian hiệu lực chỉ được chứa số");
        }
    }
}
