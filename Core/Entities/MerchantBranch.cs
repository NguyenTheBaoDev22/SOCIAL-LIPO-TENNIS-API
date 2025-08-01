using Core.Enumerables;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    [Index(nameof(MerchantId), nameof(MerchantBranchCode), IsUnique = true)]
    public class MerchantBranch : Audit,ITenantEntity
    {

        // Thêm trường SequenceNumber (tự động tăng) để giữ số thứ tự cho chi nhánh
        public int SequenceNumber { get; private set; }  // Số thứ tự của chi nhánh
        [MaxLength(3)]
        public string MerchantBranchCode { get; set; }  // Mã điểm bán hàng (3 ký tự)
        [Required]
        [MaxLength(255)]
        public string BranchName { get; set; }  // Tên điểm bán hàng
        [Required]
        public string BranchAddress { get; set; }  // Địa chỉ của điểm bán

        // Mối quan hệ 1 - N với PaymentTerminal
        public ICollection<PaymentTerminal> Terminals { get; set; }  // Các máy thanh toán tại điểm bán
        public string MerchantCode { get; set; }
        // Mối quan hệ với Merchant
        [Required]
        public Guid MerchantId { get; set; }
        public virtual Merchant Merchant { get; set; }  // Liên kết đến Merchant
        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string? BranchEmail { get; set; } // Email của riêng chi nhánh
        [Required]
        public string? BranchPhone { get; set; }//Phone  của riêng chi nhánh
        public int VerificationAttempts { get; set; } = 0;  // Số lần thẩm định của Napas (mặc định là 0)

        [MaxLength(50)]
        public string VerificationStatus { get; set; } = VerificationStatusConstants.Pending; //VerificationStatusConstants Trạng thái thẩm định của merchant
        public bool IsHeadOffice { get; set; } = false;
        public string BranchTaxNumber { get; set; }
        // Các thông tin về hình ảnh cửa hàng
        public List<string>? ExteriorImages { get; set; }  // Danh sách URL hình ảnh bên ngoài cửa hàng, bao gồm bảng hiệu thể hiện địa chỉ
        public List<string>? InteriorImages { get; set; }  // Danh sách URL hình ảnh bên trong cửa hàng, sản phẩm trưng bày, quầy tính tiền, v.v.
        [Required]
        // Các thông tin về vị trí định vị trên bản đồ
        public string ProvinceCode { get; set; }
        [Required]
        public string ProvinceName { get; set; }
        [Required]
        public string CommuneCode { get; set; }
        [Required]
        public string CommuneName { get; set; }
        public double? Latitude { get; set; }  // Vị trí vĩ độ của cửa hàng trên bản đồ (tọa độ GPS)
        public double? Longitude { get; set; }  // Vị trí kinh độ của cửa hàng trên bản đồ (tọa độ GPS)
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankAccountHolder { get; set; }
        public string Status { get; set; } = EBranchStatus.Inactive;
        public string? SignedForm01AUrl { get; set; }
        public string MerchantCategoryCode { get; set; }
        public string? ActiveCallbackUrl { get; set; }
        // ITenantEntity implementation
        public Guid TenantId { get; set; }
    }
}
