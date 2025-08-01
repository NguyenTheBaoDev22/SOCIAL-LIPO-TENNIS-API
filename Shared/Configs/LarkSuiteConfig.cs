using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configs
{
    public class LarkSuiteConfig : ILarkSuiteConfig
    {
        public string AppId { get; set; } = default!;
        public string AppSecret { get; set; } = default!;
        public string RedirectUri { get; set; } = default!;
        public string Scope { get; set; } = default!;
        public string AuthBaseUrl { get; set; } = default!;
        public string TokenEndpoint { get; set; } = default!;
        public string LarkMailApiBaseUrl { get; set; } = default!;
        public string MailboxAddress { get; set; } = default!;
        public string RefreshTokenEndpoint { get; set; }=default!;
    }
}
