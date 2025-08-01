namespace Applications.DTOs
{
    public class AccountCreationSMSOtp
    {
        public string PhoneNumber { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
        //public string Url { get; set; } = null!; // Địa chỉ URL (VD: https://zenpay.vn)
        public string ExpirationInMinutes { get; set; } = null!; // Thời gian hiệu lực OTP (ví dụ: 5 phút)

        /// <summary>
        /// Tạo thông điệp SMS cho OTP
        /// </summary>
        public string ToSmsOtpMessage()
        {
            //return $"Mã xác thực tài khoản ZENPAY của bạn tại {Url} là: {OtpCode}. " +
            //       $"Tuyệt đối KHÔNG chia sẻ mã này cho bất kỳ ai dưới bất kỳ hình thức nào. " +
            //       $"Mã có hiệu lực trong {ExpirationInMinutes} phút.";
            return $"Ma xac thuc tai khoan ZENPAY cua ban tai www.zenpay.com.vn la: {OtpCode} Tuyet doi KHONG chia se ma xac thuc cho bat ky ai duoi bat ky hinh thuc nao. Ma xac thuc co hieu luc trong {ExpirationInMinutes} phut.";
        }


        //        Ma xac thuc tai khoan ZENPAY cua ban tai www.zenpay.com.vn la: .{ 0,10}
        //        Tuyet doi KHONG chia se ma xac thuc cho bat ky ai duoi bat ky hinh thuc nao.Ma xac thuc co hieu luc trong .{ 0,5}
        //        phut.

        //ZenPay xac nhan cua hang.{0,50}
        //    da dang ky thanh cong.{0,10}
        //Chu cua hang.{0,50}Ma cua hang.{0,30}So dien thoai.{0,20}Dia chi.{0,80}
        //Hay lien he voi ZenPay theo hotline .{0,20} neu Quy khach can them ho tro tu van ve dich vu!
    }
}
