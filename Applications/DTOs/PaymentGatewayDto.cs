using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.DTOs
{
    /// <summary>
    /// Kết quả trả về từ PG sau khi tạo QR.
    /// </summary>
    public class PaymentQrResponse
    {
        /// <summary>Mã QR dạng text</summary>
        public string QrContent { get; set; } = default!;

        /// <summary>Thời điểm hết hạn của mã QR</summary>
        public DateTime ExpiredAt { get; set; }

        /// <summary>Mã giao dịch của PG, nếu có</summary>
        public string? TransactionId { get; set; }
    }
    public class ZenPayQrResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = default!;
        public string Data { get; set; } = default!;
        public string? TraceId { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
