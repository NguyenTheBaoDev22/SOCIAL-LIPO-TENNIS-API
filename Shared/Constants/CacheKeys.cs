using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Constants
{
    public static class CacheKeys
    {
        public const string LarkToken = "lark_token";
        public const string GoogleToken = "google_token";
        public const string ZaloToken = "zalo_token";
        // Nếu cần multi-tenant, multi-user thì format template:
        public const string LarkTokenByUser = "lark_token_{0}";
        public const string LarkTokenByTenant = "lark_token_{0}";
        // ... bổ sung các key cho Google, Zalo, Config, Lookup ...
        // ... các key khác
    }
}
