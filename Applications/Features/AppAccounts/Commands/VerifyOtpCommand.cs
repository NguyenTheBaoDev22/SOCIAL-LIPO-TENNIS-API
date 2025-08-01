using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.AppAccounts.Commands
{
    public class VerifyOtpCommand : IRequest<BaseResponse<string>>
    {
        public string PhoneNumber { get; set; } = default!;
        public string OtpCode { get; set; } = default!;
    }
}
