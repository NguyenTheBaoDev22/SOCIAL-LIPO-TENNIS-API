using Applications.Features.Notifications.Commands;
using Applications.Interfaces.Services;
using MediatR;
using Shared.Results;

namespace Applications.Features.Notifications.Handlers
{
    public class SendAccountRegistrationSmsOtpHandler : IRequestHandler<SendAccountRegistrationSmsOtpCommand, BaseResponse<bool>>
    {
        private readonly IMBFSmsBranchnameService _smsService;

        public SendAccountRegistrationSmsOtpHandler(IMBFSmsBranchnameService smsService)
        {
            _smsService = smsService;
        }

        public async Task<BaseResponse<bool>> Handle(SendAccountRegistrationSmsOtpCommand request, CancellationToken cancellationToken)
        {
            //var result = await _smsService.SendAccountRegistrationNonUnicodeSMSOtpAsync(request.SmsOtp);
            var result = await _smsService.SendAccountRegistrationNonUnicodeSMSOtpAsync(request.SmsOtp);
            return result; // Chuyển trả về từ service (Success/Failure)
        }
    }
}
