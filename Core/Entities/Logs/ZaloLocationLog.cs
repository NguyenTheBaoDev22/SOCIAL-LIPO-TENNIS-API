using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Logs
{
    public class ZaloLocationLog : Audit
    {
        public string TraceId { get; set; }
        public string RequestUrl { get; set; }
        public string Token { get; set; }
        public string? Coordinates { get; set; } // "lat,long"
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CalledAt { get; set; }
    }
}
