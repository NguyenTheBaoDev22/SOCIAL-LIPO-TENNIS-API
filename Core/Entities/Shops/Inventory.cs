using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Shops
{
    public class ShopProductInventory:Audit
    {
        [Required]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = default!;

        [Required]
        public DateTime AuditDate { get; set; }

        [Required]
        public int ExpectedQty { get; set; }

        [Required]
        public int ActualQty { get; set; }

        [NotMapped]
        public int AdjustmentQty => ActualQty - ExpectedQty;

        [MaxLength(500)]
        public string? Reason { get; set; }

        [MaxLength(255)]
        public string? AuditorName { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public Guid MerchantId { get; set; }

        [Required]
        public Guid MerchantBranchId { get; set; }
    }
}

