using System.ComponentModel.DataAnnotations;

namespace Applications.DTOs
{
    public class SMSAccountCreationConfirmationRequestDto
    {
        [Required(ErrorMessage = "Tên cửa hàng là bắt buộc")]
        [MaxLength(20, ErrorMessage = "Tên cửa hàng không được vượt quá 20 ký tự")]
        public string ShopName { get; set; }

        [Required(ErrorMessage = "Mã cửa hàng là bắt buộc")]
        [MaxLength(20, ErrorMessage = "Mã cửa hàng không được vượt quá 20 ký tự")]
        public string ShopCode { get; set; }

        [Required(ErrorMessage = "Tên chủ cửa hàng là bắt buộc")]
        [MaxLength(35, ErrorMessage = "Tên chủ cửa hàng không được vượt quá 35 ký tự")]
        public string ShopOwnerName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải gồm đúng 10 chữ số")]
        public string ShopOwnerPhone { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [MaxLength(80, ErrorMessage = "Địa chỉ không được vượt quá 80 ký tự")]
        public string ShopAddress { get; set; }
    }
}
