using Applications.Features.LarksuiteIntegrations.DTOs;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    public interface ILarkEmailService
    {
        Task<BaseResponse<string>> SendEmailAsync(LarkEmailDto dto, string accessToken, CancellationToken cancellationToken = default);
    }
}
