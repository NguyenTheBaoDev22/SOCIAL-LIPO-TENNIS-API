using Shared.Utils;

namespace Applications.DTOs
{
    public class AccountCreationSMSConfirmation
    {
        public string ShopOwnerPhone { get; set; } = null!;
        public string ShopOwnerName { get; set; } = null!;
        public string ShopName { get; set; } = null!;
        public string ShopCode { get; set; } = null!;
        public string ShopAddress { get; set; } = null!;

        /// <summary>
        /// Tạo thông điệp SMS cho xác nhận tài khoản cửa hàng
        /// </summary>
        public string ToNonUnicodeConfirmationMessage()
        {
            var message = $"ZenPay xac nhan cua hang {ShopName} da dang ky thanh cong\n" +
                   $"Chu cua hang {ShopOwnerName}\n" +
                   $"Ma cua hang {ShopCode}\n" +
                   $"So dien thoai {ShopOwnerPhone}\n" +
                   $"Dia chi {ShopAddress}\n" +
                   $"Hay lien he voi ZenPay theo hotline 0399686878 de duoc ho tro.";
            // Loại bỏ dấu tiếng Việt trong thông điệp SMS, kiểm tra nếu chuỗi không null
            return !string.IsNullOrEmpty(message) ? StringUtils.RemoveVietnameseAccent(message) : string.Empty;
        }
        public string ToUnicodeConfirmationMessage()
        {
            //return $"Chúc mừng {ShopOwnerName} - chủ cửa hàng {ShopName}. " +
            //       $"Mã cửa hàng của bạn: {ShopCode}. Địa chỉ: {ShopAddress}. " +
            //       $"Chúc bạn thành công với ZENPAY!";
            return $"ZenPay xác nhận cửa hàng {ShopName} đã đăng ký thành công\nChủ cửa hàng {ShopOwnerName}\nMã cửa hàng {ShopCode}\nSố điện thoại {ShopOwnerPhone}\nĐịa chỉ {ShopAddress}\nHãy liên hệ với ZenPay theo hotline 0399.68.68.78 nếu Quý khách cần thêm hỗ trợ tư vấn về dịch vụ!";
        }
    }
}
