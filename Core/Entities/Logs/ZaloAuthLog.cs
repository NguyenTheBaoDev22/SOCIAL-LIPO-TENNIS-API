using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Logs
{
    public class ZaloAuthLog : Audit
    {
        public string TraceId { get; set; } = default!;
        public string RequestUrl { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CalledAt { get; set; } = DateTime.UtcNow;
    }
}
