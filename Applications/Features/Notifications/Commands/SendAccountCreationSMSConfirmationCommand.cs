using Applications.DTOs;
using MediatR;
using Shared.Results;

namespace Applications.Features.Notifications.Commands
{
    // Command gửi thông báo SMS xác nhận tài khoản cửa hàng
    public class SendAccountCreationSMSConfirmationCommand : IRequest<BaseResponse<bool>>
    {
        public AccountCreationSMSConfirmation SmsConfirmation { get; set; }
    }
}
