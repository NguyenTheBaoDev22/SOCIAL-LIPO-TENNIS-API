using Core.Enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Partners
{
    /// <summary>
    /// Log các lần ZenShop gửi callback về IPN của đối tác
    /// </summary>
    public class PartnerMerchantStatusCallbackLog : Audit
    {
        public string CallbackUrl { get; set; } = null!;
        public string Payload { get; set; } = null!;
        public int HttpStatusCode { get; set; }
        public string? ResponseContent { get; set; }

        public bool IsSuccess { get; set; }
        public string Status { get; set; } = MerchantCallbackStatus.Pending; // Pending, Success, Failed, Timeout, Cancelled

        public DateTime? SentAt { get; set; }
        public int RetryCount { get; set; } = 0;

    }
}
