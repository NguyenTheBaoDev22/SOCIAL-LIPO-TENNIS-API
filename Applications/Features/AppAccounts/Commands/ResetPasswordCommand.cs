using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.AppAccounts.Commands
{
    public class ResetPasswordCommand : IRequest<BaseResponse<bool>>
    {
        public string PhoneNumber { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
