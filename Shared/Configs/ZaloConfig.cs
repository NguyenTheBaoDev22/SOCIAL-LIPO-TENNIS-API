using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configs
{
    public class ZaloConfig
    {
        public string AppId { get; set; } = default!;
        public string AppSecretKey { get; set; } = default!;
        public string BaseUrl { get; set; } = "https://graph.zalo.me";

        // Optional – nếu bạn cần cấu hình thêm
        public string RedirectUri { get; set; } = default!;
        public string GetUserInfoEndpoint { get; set; } = default!;
    }
}
