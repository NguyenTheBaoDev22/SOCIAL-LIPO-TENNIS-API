using Core.Enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Partners
{
    public class PartnerTransactionCallbackLog : Audit
    {
        public Guid PartnerOrderId { get; set; }
        public string CallbackUrl { get; set; } = null!;
        public string Payload { get; set; } = null!;
        public int HttpStatusCode { get; set; }
        public string? ResponseContent { get; set; }
        public bool IsSuccess { get; set; }
        public string Status { get; set; } = MerchantCallbackStatus.Pending;
        public DateTime? SentAt { get; set; }
        public int RetryCount { get; set; } = 0;

        public PartnerOrder? PartnerOrder { get; set; }
    }
}
