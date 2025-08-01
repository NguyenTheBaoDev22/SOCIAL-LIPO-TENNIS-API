using Core.Enumerables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Partners
{
    /// <summary>
    /// Đơn hàng được tạo bởi Partner để yêu cầu ZenShop sinh QR thanh toán
    /// </summary>
    public class PartnerOrder : Audit
    {
        public Guid? PartnerId { get; set; }
        public string? PartnerCode { get; set; } = null!;

        // === Các trường từ NapasQrGenReq ===
        public string MerchantCode { get; set; } = null!;
        public string MerchantBranchCode { get; set; } = null!;
        public string PaymentTerminalCode { get; set; } = null!;
        public string QrType { get; set; } = "QRIBFTTA";
        public int Amount { get; set; }

        [MaxLength(500)]
        public string? PurposeOfTransaction { get; set; }

        [MaxLength(100)]
        public string? OrderCode { get; set; }

        [MaxLength(1000)]
        public string Ipn { get; set; } = null!;

        // === Thông tin QR sinh ra ===
        public string? QrContent { get; set; }
        public DateTime? QrExpiredAt { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = PartnerOrderStatus.Pending;

        // === Thông tin log HTTP Request gốc ===
        [MaxLength(50)]
        public string? SourceIpAddress { get; set; }

        [MaxLength(1000)]
        public string? UserAgent { get; set; }

        public string? RequestRawJson { get; set; }
        public bool? IpnSent { get; set; }                    // true nếu IPN đã gửi ít nhất 1 lần
        public bool? IpnSuccess { get; set; }                 // true nếu IPN thành công (200 OK)
        public DateTime? IpnLastAttemptAt { get; set; }       // lần cuối ZS gọi IPN
        public int IpnRetryCount { get; set; }                // số lần retry
        // === Navigation ===
        public Partner? Partner { get; set; }

        public ICollection<PartnerTransactionCallbackLog> PartnerTransactionCallbackLogs { get; set; }    }

}
