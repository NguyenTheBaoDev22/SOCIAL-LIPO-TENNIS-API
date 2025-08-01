using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Integrations
{
    public class LarkEmailLog:Audit
    {
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string Recipient { get; set; } = null!;
        public string From { get; set; } = null!;
        public string Response { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
