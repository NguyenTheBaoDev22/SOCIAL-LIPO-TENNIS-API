using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.ZaloIntegrations.ZaloAuth.DTOs
{
    public class UserZaloIdentityResponseModel
    {
        public string UserPhoneNumber { get; set; }
    }
    public class ZaloPhoneNumberData
    {
        [JsonProperty("number")]
        public string? ZaloUserPhoneNumber { get; set; }
    }
}
