using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.ZaloIntegrations.ZaloAuth.Queries
{
    public class GetZaloLocationQuery : IRequest<BaseResponse<LocationResultDto>>
    {
        public string Code { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
    }
}
