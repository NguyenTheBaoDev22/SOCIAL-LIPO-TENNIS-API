using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class OtpRequestCounter:Audit
    {
        public string PhoneNumber { get; set; } = default!;
        public string Purpose { get; set; } = default!;
        public int Count { get; set; } = 0;
        public DateTime LastResetAt { get; set; } = DateTime.UtcNow;
    }
}
