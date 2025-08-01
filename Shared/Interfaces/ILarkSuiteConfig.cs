using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface ILarkSuiteConfig
    {
        string AppId { get; }
        string AppSecret { get; }
        string RedirectUri { get; }
        string Scope { get; }
        string AuthBaseUrl { get; }
        string TokenEndpoint { get; }
        string LarkMailApiBaseUrl { get; }
        string MailboxAddress { get; }
        string RefreshTokenEndpoint { get; }
    }
}
