using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    /// <summary>
    /// Entity lưu thông tin access token và refresh token lấy từ Lark OAuth2.
    /// </summary>

    public class LarkTokens : Audit
    {
        /// <summary>
        /// Access Token nhận từ Lark (text)
        /// </summary>
        [Required]
        public string AccessToken { get; set; } = default!;

        /// <summary>
        /// Refresh Token nhận từ Lark (text)
        /// </summary>
        [Required]
        public string RefreshToken { get; set; } = default!;

        /// <summary>
        /// Thời điểm hết hạn access token (timestamp with time zone)
        /// </summary>
        [Column(TypeName = "timestamp with time zone")]
        public DateTime AccessTokenExpiresAt { get; set; }

        /// <summary>
        /// Thời điểm hết hạn refresh token (timestamp with time zone)
        /// </summary>
        [Column(TypeName = "timestamp with time zone")]
        public DateTime RefreshTokenExpiresAt { get; set; }

        /// <summary>
        /// Trạng thái xử lý token (text, required)
        /// </summary>
        [Required]
        public string Status { get; set; } = default!;

        /// <summary>
        /// Thời điểm gửi request tới Lark (timestamp with time zone)
        /// </summary>
        [Column(TypeName = "timestamp with time zone")]
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// Dữ liệu raw JSON trả về từ Lark (text)
        /// </summary>
        [Required]
        public string RawData { get; set; } = default!;
    }
}
