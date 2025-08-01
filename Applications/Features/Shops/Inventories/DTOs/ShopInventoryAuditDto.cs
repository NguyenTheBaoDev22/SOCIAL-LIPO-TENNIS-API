using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Inventories.DTOs
{
    public class ShopInventoryAuditDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public DateTime AuditDate { get; set; }
        public int ExpectedQty { get; set; }
        public int ActualQty { get; set; }
        public int AdjustmentQty => ActualQty - ExpectedQty;
        public string? Reason { get; set; }
        public string? AuditorName { get; set; }
    }
}
