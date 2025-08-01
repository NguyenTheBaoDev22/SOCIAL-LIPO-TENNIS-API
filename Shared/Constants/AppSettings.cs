using Shared.Interfaces.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Constants
{
    public class AppSettings : IAppSettings
    {
        public FrontendUrlConfig FrontendUrlConfig { get; set; }
        public string SupportEmail { get; set; } = null!;
        public int TokenExpiryMinutes { get; set; } = 60;
    }

    public class FrontendUrlConfig
    {
        public string BaseUrl { get; set; } // Thêm thuộc tính BaseUrl
    }
}
