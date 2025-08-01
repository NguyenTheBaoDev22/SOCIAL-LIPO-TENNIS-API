using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Tenants
{
    /// <summary>
    /// Entity base có phân tầng Tenant → dùng cho toàn bộ entity có Tenant/Merchant/Branch.
    /// </summary>
    public abstract class TenantEntity : Audit
    {
        /// <summary>
        /// Tổ chức sở hữu (Tenant)
        /// </summary>
         [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// Merchant được gán
        /// </summary>
        /// 
        [Required]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Chi nhánh được gán
        /// </summary>
        [Required]
        public Guid MerchantBranchId { get; set; }
    }
}
