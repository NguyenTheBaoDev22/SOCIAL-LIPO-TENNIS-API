using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Shops
{
    public class ProductImport : Audit
    {
        [Required]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = default!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImportPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime ImportDate { get; set; }

        [MaxLength(255)]
        public string? SupplierName { get; set; }

        [MaxLength(50)]
        public string? PurchaseOrderCode { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public Guid MerchantId { get; set; }

        [Required]
        public Guid MerchantBranchId { get; set; }
    }
}
