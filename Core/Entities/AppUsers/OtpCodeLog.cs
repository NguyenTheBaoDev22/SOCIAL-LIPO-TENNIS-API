using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.AppUsers
{

    /// <summary>
    /// Lưu lịch sử OTP được gửi đi để phục vụ xác thực và chống spam.
    /// </summary>
    public class OtpCodeLog : Audit
    {
        /// <summary>
        /// Số điện thoại đích nhận OTP.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Mã OTP được gửi.
        /// </summary>
        public string OtpCode { get; set; }

        /// <summary>
        /// Thời gian mã OTP hết hạn.
        /// </summary>
        public DateTime ExpireAt { get; set; }

        /// <summary>
        /// Đánh dấu OTP đã được xác minh.
        /// </summary>
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Mục đích gửi OTP (ví dụ: ForgotPassword, Register, Login...).
        /// </summary>
        public string Purpose { get; set; }
    }

}
