using Applications.DTOs;
using Applications.Interfaces.Services;
using Shared.Results;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMBFSmsBranchnameService _smsService;

        public NotificationService(IMBFSmsBranchnameService smsService)
        {
            _smsService = smsService;
        }

        public async Task<BaseResponse<bool>> SendAccountRegistrationSMSOtpAsync(AccountCreationSMSOtp otp)
        {
            return await _smsService.SendAccountRegistrationUnicodeSMSOtpAsync(otp); // Delegate to SMS service
        }

        public Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            Console.WriteLine($"📧 [FakeEmail] To: {to}, Subject: {subject}, Content: {htmlContent}");
            return Task.CompletedTask;
        }

        public Task SendSmsAsync(string phoneNumber, string message)
        {
            Console.WriteLine($"📱 [FakeSMS] To: {phoneNumber}, Message: {message}");
            return Task.CompletedTask;
        }

        // Implement the SendResetPasswordEmail method
        public Task SendResetPasswordEmail(string email, string resetUrl)
        {
            // Fake implementation for demo purposes
            Console.WriteLine($"📧 [Reset Password Email] To: {email}, Reset URL: {resetUrl}");
            return Task.CompletedTask;
        }
    }
}
