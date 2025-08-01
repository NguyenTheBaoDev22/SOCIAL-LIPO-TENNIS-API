using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enumerables
{

    /// <summary>
    /// Trạng thái đơn hàng được tạo từ Partner (string-based enum)
    /// </summary>
    public static class PartnerOrderStatus
    {
        public const string Pending = "Pending";           // Đã nhận yêu cầu, chưa tạo QR
        public const string QrGenerated = "QrGenerated";   // Đã sinh mã QR
        public const string Paid = "Paid";                 // Đã thanh toán
        public const string Expired = "Expired";           // QR hết hạn
        public const string Cancelled = "Cancelled";       // Đơn bị huỷ
        public const string Failed = "Failed";             // Lỗi trong quá trình xử lý

        /// <summary>
        /// Tập hợp tất cả giá trị hợp lệ
        /// </summary>
        public static readonly string[] All = new[]
        {
        Pending,
        QrGenerated,
        Paid,
        Expired,
        Cancelled,
        Failed
    };
    }
}
