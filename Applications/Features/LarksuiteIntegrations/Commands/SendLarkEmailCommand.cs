using Applications.Features.LarksuiteIntegrations.DTOs;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.LarksuiteIntegrations.Commands
{
    public class SendLarkEmailCommand : IRequest<BaseResponse<string>>
    {
        public string AccessToken { get; set; } = default!;
        public LarkEmailDto Email { get; set; } = default!;
    }
}
