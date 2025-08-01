using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Shops
{
    public class ProductCategory:Audit
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // Multi-tenant
        [Required]
        public Guid TenantId { get; set; }
        [Required]
        public Guid MerchantId { get; set; }
        [Required]
        public Guid MerchantBranchId { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
