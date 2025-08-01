using Core.Enumerables;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{

    [Index(nameof(MerchantBranchId), nameof(TerminalCode), IsUnique = true)] // Có thể thiết lập duy nhất trong phạm vi chi nhánh nếu cần
    public class PaymentTerminal : Audit, ITenantEntity
    {
        [Required]
        [MaxLength(2)] // Độ dài tối đa 2 ký tự
        [RegularExpression("^[0-9]{2}$", ErrorMessage = "Mã thiết bị phải là 2 chữ số (từ 00 đến 99).")]
        public string TerminalCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string TerminalName { get; set; } // Tên thiết bị (ví dụ: "Máy POS Quầy 1", "Soundbox Lối vào")

        [MaxLength(50)]
        public string DeviceType { get; set; } = DeviceTypeConstants.Soundbox; // Loại thiết bị (ví dụ: "POS", "Soundbox", "Tablet", "QR Standee")
                                                                               // Nên dùng enum hoặc lookup table cho loại này

        [MaxLength(255)]
        public string SerialNumber { get; set; } // Số serial của thiết bị, giúp quản lý hàng tồn kho hoặc bảo hành
        [MaxLength(255)]
        public string? IMEI { get; set; }
        [MaxLength(255)]
        public string? Manufacturer { get; set; } // Nhà sản xuất thiết bị (ví dụ: "Pax", "Ingenico", "Sunmi")

        [MaxLength(50)]
        public string? Model { get; set; } // Model của thiết bị (ví dụ: "A920", "P400", "L2")

        [MaxLength(50)]
        public string Status { get; set; } = TerminalStatusConstants.Inactive; // Trạng thái hoạt động của thiết bị (Active, Inactive, Damaged, Lost)
        public Guid MerchantId { get; set; }
        public virtual Merchant Merchant { get; set; }
        public string MerchantCode { get; set; }
        public Guid MerchantBranchId { get; set; }
        public virtual MerchantBranch MerchantBranch { get; set; } // Khóa ngoại liên kết với MerchantBranch
        public string MerchantBranchCode { get; set; }
        // Các thông tin cấu hình khác của thiết bị (tùy theo nhu cầu)
        public string? FirmwareVersion { get; set; } // Phiên bản firmware
        public DateTime? LastSyncDate { get; set; } // Ngày cuối cùng đồng bộ dữ liệu
        public string? DeviceId { get; set; } // Địa chỉ IP nếu là thiết bị mạng

        // --- TRƯỜNG ĐỊNH DANH TỔNG HỢP MỚI (SẼ LƯU VÀO DB) ---

        /// <summary>
        /// Mã định danh tổng hợp (MerchantCode-MerchantBranchCode-TerminalCode).
        /// Ví dụ: "ABC-XYZ-01"
        /// </summary>
        [MinLength(12)]
        [MaxLength(12)] // 3 (MerchantCode) + 1 (-) + 3 (BranchCode) + 1 (-) + 2 (TerminalCode) = 10 ký tự
                        // Thêm [Required] nếu bạn muốn đảm bảo nó luôn có giá trị trong DB
                        // [Required] 
        public string CombinedIdentifier { get; set; }
        // Chúng ta sẽ gán giá trị cho nó thông qua logic nghiệp vụ thay vì qua getter tính toán tự động.
        // Dùng string? nếu bạn muốn nó có thể null trong DB, hoặc string nếu [Required]




        // Có thể thêm trường để lưu cấu hình riêng cho từng thiết bị
        // Ví dụ: Json của các cài đặt đặc biệt
        public string? ConfigurationJson { get; set; }
        public string? ActiveCallbackUrl { get; set; }
        //// --- TRƯỜNG ĐỊNH DANH TỔNG HỢP MỚI ---

        ///// <summary>
        ///// Mã định danh tổng hợp (MerchantCode-MerchantBranchCode-TerminalCode).
        ///// Ví dụ: "ABC-XYZ-01"
        ///// </summary>
        //[NotMapped] // Đánh dấu là thuộc tính không ánh xạ vào DB. Nó sẽ được tính toán khi truy xuất.
        //            // Hoặc bạn có thể lưu nó vào DB nếu muốn tìm kiếm nhanh hơn và không muốn tính toán lại mỗi lần.
        //            // Nếu lưu vào DB, bỏ [NotMapped] và thêm [MaxLength] phù hợp.
        //[MaxLength(10)] // 3 (MerchantCode) + 1 (-) + 3 (BranchCode) + 1 (-) + 2 (TerminalCode) = 10 ký tự
        //public string CombinedIdentifier
        //{
        //    get
        //    {
        //        // Đảm bảo các navigation property đã được load (eager loading hoặc lazy loading)
        //        // hoặc các mã đã có sẵn trước khi truy cập.
        //        // Nếu MerchantBranch hoặc Merchant là null, có thể trả về null hoặc một chuỗi lỗi/rỗng.
        //        if (MerchantBranch?.Merchant == null || MerchantBranch == null)
        //        {
        //            // Xử lý trường hợp dữ liệu chưa được load đầy đủ
        //            return null; // Hoặc string.Empty, hoặc throw new InvalidOperationException("Related data not loaded.");
        //        }
        //        return $"{MerchantBranch.Merchant.ZenPayMasterMerchantCode}{MerchantBranch.Merchant.MerchantCode}{MerchantBranch.MerchantBranchCode}{TerminalCode}";
        //    }
        //    // Setter có thể không cần thiết nếu đây là trường chỉ đọc được tính toán.
        //    // Nếu bạn muốn lưu vào DB, bạn sẽ cần setter để gán giá trị.
        //}
        // ITenantEntity implementation
        public Guid TenantId { get; set; }
    }
}
