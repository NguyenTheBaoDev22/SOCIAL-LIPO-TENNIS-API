using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.ZaloIntegrations.ZaloAuth.DTOs
{
    public class ZaloBaseResponse<T>
    {
        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess => Error == 0;
    }
}
