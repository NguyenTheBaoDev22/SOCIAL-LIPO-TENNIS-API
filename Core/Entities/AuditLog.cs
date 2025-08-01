using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class AuditLog : Audit // hoặc Audit nếu bạn muốn soft-delete
    {
        public string EntityName { get; set; } = null!;
        public string EntityId { get; set; } = null!; // string để hỗ trợ cả Guid/int
        public string ActionType { get; set; } = null!; // "Create", "Update", "Delete"
        public string? Changes { get; set; } // JSON string mô tả chi tiết field thay đổi

        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        public string? TraceId { get; set; }
        public Guid? TenantId { get; set; }
    }
}
