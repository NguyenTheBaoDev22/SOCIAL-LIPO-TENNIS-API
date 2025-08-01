using Applications.Features.Notifications.Commands;
using Applications.Interfaces.Services;
using MediatR;
using Shared.Results;

namespace Applications.Features.Notifications.Handlers
{
    public class SendAccountCreationSMSConfirmationHandler : IRequestHandler<SendAccountCreationSMSConfirmationCommand, BaseResponse<bool>>
    {
        private readonly IMBFSmsBranchnameService _smsService;

        public SendAccountCreationSMSConfirmationHandler(IMBFSmsBranchnameService smsService)
        {
            _smsService = smsService;
        }

        public async Task<BaseResponse<bool>> Handle(SendAccountCreationSMSConfirmationCommand request, CancellationToken cancellationToken)
        {
            //var result = await _smsService.SendAccountCreationNonUnicodeSMSConfirmationAsync(request.SmsConfirmation);
            var result = await _smsService.SendAccountCreationNonUnicodeSMSConfirmationAsync(request.SmsConfirmation);
            return result; // Trả về kết quả gửi SMS từ service
        }
    }

}
