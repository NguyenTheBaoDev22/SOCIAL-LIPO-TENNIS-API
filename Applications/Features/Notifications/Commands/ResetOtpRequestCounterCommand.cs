using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Notifications.Commands
{
    public class ResetOtpRequestCounterCommand : IRequest<BaseResponse<bool>>
    {
        public string PhoneNumber { get; set; } = default!;
        public string Purpose { get; set; } = default!;
    }
}
