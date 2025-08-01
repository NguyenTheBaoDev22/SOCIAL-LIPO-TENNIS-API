using System.ComponentModel.DataAnnotations;

namespace Applications.DTOs
{
    public class SmsOTPRequestDto
    {
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa số")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Mã OTP là bắt buộc")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "OTP phải đúng 4 chữ số")]
        public string OTP { get; set; } = null!;

        [Required(ErrorMessage = "Thời gian hiệu lực là bắt buộc")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Thời gian hiệu lực chỉ được chứa số")]
        public string ExpirationInMinutes { get; set; } = null!;
    }
}
