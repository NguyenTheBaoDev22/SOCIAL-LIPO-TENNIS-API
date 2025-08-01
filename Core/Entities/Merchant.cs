using Core.Entities.Partners;
using Core.Enumerables;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    [Index(nameof(MerchantCode), IsUnique = true)]
    [Index(nameof(BusinessRegistrationNo), IsUnique = true)]
    [Index(nameof(PrimaryEmail))]
    public class Merchant : Audit, ITenantEntity
    {

        // Cột số nguyên chỉ để làm số thứ tự
        public int SequenceNumber { get; private set; }
        // Cột mã sẽ được tính toán tự động, không cần chỉnh sửa thủ công
        public string MerchantCode { get; private set; }
        [Required]
        [MaxLength(255)]
        public string MerchantName { get; set; }  // Tên của merchant
        [Required]
        [MaxLength(255)]
        public string BusinessRegistrationNo { get; set; }  // Số đăng ký kinh doanh của merchant
        [Required]
        public List<string> BusinessRegistrationImageUrls { get; set; }
        [Required]
        public string PrimaryTaxNumber { get; set; }
        [MaxLength(255)]
        public string BusinessAddress { get; set; }  // Địa chỉ của doanh nghiệp hoặc hộ kinh doanh
        [Required]
        [MaxLength(255)]
        public string OwnerName { get; set; }  // Tên chủ sở hữu hoặc người đại diện
        public string OwnerIdCardNumber { get; set; }  // Số CCCD hoặc người đại diện
        public string OwnerIdCardFrontUrl { get; set; }
        public string OwnerIdCardBackUrl { get; set; }
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string PrimaryEmail { get; set; }  // Địa chỉ email của merchant
        [Required]
        [MaxLength(20)]
        public string PrimaryPhone { get; set; }  // Số điện thoại của merchant

        [Required]
        [MaxLength(50)]
        public string MerchantType { get; set; }  // Loại merchant: "Hộ kinh doanh" hoặc "Doanh nghiệp công ty"
        [MaxLength(50)]
        // Navigation property đến các chi nhánh
        public bool IsActive { get; set; } = false;
        public virtual ICollection<MerchantBranch> Branches { get; set; } = new List<MerchantBranch>();
        public string ZenPayMasterMerchantCode { get; set; } = "NPHD";
        /// <summary>
        /// Nguồn đăng ký merchant (Partner, SelfRegistered, Marketing, ZSDevelopment, ...)
        /// </summary>
        [MaxLength(50)]
        public string MerchantSource { get; set; } = MerchantSourceConstants.ZSDevelopment;

        /// <summary>
        /// Đối tác (nếu có) đã tạo merchant này
        /// </summary>
        public Guid? PartnerId { get; set; }

        public Partner? Partner { get; set; } // Navigation property (nếu bạn đã có entity Partner)
        public DateTime? VerifiedAt { get; set; }
        public string? VerifiedBy { get; set; }
     /// Mỗi merchant cần có một TenantId để xác định thuộc về tenant nào
        // ITenantEntity implementation
        public Guid TenantId { get; set; }
    }

}
