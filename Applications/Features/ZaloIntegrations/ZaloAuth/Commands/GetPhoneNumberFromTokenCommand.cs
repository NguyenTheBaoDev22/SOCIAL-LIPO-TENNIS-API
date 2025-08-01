using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using MediatR;
using Newtonsoft.Json;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.ZaloIntegrations.ZaloAuth.Commands
{

    public class GetPhoneNumberFromTokenCommand : IRequest<BaseResponse<UserZaloIdentityResponseModel>>
    {

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("phoneTokenCode")]
        public string PhoneTokenCode { get; set; }
    }
}
