using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Options
{
    public class VietQrOptions
    {
        public const string SectionName = "ExternalApis:VietQr";
        public string BaseUrl { get; set; } = string.Empty;
        public string TaxCodeLookupEndpoint { get; set; } = string.Empty;
    }
}
