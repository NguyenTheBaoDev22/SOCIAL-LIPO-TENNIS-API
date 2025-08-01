using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Options
{
    public class IpRateLimitOptions
    {
        public bool EnableEndpointRateLimiting { get; set; }
        public bool StackBlockedRequests { get; set; }
        public int HttpStatusCode { get; set; }
        public string RealIpHeader { get; set; } = "X-Real-IP";
        public string ClientIdHeader { get; set; } = "X-ClientId";
        public bool DisableRateLimitHeaders { get; set; }
        public List<RateLimitRule> GeneralRules { get; set; } = new();
    }

    public class RateLimitRule
    {
        public string Endpoint { get; set; } = "*";
        public string Period { get; set; } = "1m";
        public int Limit { get; set; }
    }
}
