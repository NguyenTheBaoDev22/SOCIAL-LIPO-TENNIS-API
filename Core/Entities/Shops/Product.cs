using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Shops
{
    public class Product : Audit
    {
        [Required, MaxLength(80)]
        public string ProductCode { get; set; } = default!;

        [Required, MaxLength(255)]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Guid ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; } = default!;

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostPrice { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ImportPrice { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SellingPrice { get; set; }

        [MaxLength(50)]
        public string? Unit { get; set; }

        public int StockQuantity { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public Guid MerchantId { get; set; }

        [Required]
        public Guid MerchantBranchId { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<ProductImport> ProductImports { get; set; } = new List<ProductImport>();
        public virtual ICollection<ShopProductInventory> InventoryAudits { get; set; } = new List<ShopProductInventory>();
    }
}
