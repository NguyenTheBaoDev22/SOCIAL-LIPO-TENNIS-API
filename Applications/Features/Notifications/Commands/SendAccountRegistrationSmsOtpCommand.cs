using Applications.DTOs;
using MediatR;
using Shared.Results;

namespace Applications.Features.Notifications.Commands
{
    public class SendAccountRegistrationSmsOtpCommand : IRequest<BaseResponse<bool>>
    {
        public AccountCreationSMSOtp SmsOtp { get; set; }
    }
}
