using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Partners
{
    /// <summary>
    /// Đối tác tích hợp (Partner) kết nối vào hệ thống ZenShop
    /// </summary>
    public class Partner : Audit
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; } = null!; // e.g. "TIKI", "LAZADA"
        public string Name { get; set; } = null!;
        public string? CallbackIpnUrl { get; set; }
        public string? PublicKey { get; set; }

        public bool IsActive { get; set; } = true;
        public string? Description { get; set; }

        // Navigation
        public ICollection<PartnerOrder> PartnerOrders { get; set; } = new List<PartnerOrder>();
        public ICollection<ClientCredential> ClientCredentials { get; set; } = new List<ClientCredential>();
    }
}
