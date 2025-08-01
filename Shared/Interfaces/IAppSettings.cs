using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{

   namespace Shared.Interfaces
{
    // ⚠️ Ghi chú quan trọng:
    // Những config như S3Setting, MailSetting, KafkaSetting, RedisSetting → nên dùng IOptions<T> với Configure<T>().
    // Vì các cấu hình này thường chỉ dùng cục bộ trong 1 service cụ thể, không cần inject toàn hệ thống.

    // 🔧 Ngược lại, các cấu hình dùng xuyên suốt toàn hệ thống (như đường dẫn frontend, email hỗ trợ, thời hạn token...) 
    // → nên gom vào AppSettings và expose qua interface IAppSettings, để dễ mock/test và dùng lại mọi nơi.

    /// <summary>
    /// Interface đại diện cho cấu hình toàn cục của hệ thống.
    /// Các thuộc tính trong đây thường được ánh xạ từ file cấu hình appsettings.json → AppSettings section.
    /// </summary>
    public interface IAppSettings
    {
            /// <summary>
            /// Đường dẫn tới hệ thống frontend (VD: https://merchant.zenshop.vn)
            /// Dùng để tạo link trong email xác thực, reset mật khẩu, v.v.
            /// </summary>
            FrontendUrlConfig FrontendUrlConfig { get; set; }

            /// <summary>
            /// Email hỗ trợ chính thức của hệ thống, dùng làm địa chỉ người gửi trong các thông báo hệ thống.
            /// </summary>
            string SupportEmail { get; }

        /// <summary>
        /// Thời gian hết hạn (phút) cho các loại token như reset password, confirm email...
        /// </summary>
        int TokenExpiryMinutes { get; }
    }
}

}
