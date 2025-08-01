using Applications.DTOs;
using Shared.Results;

namespace Applications.Interfaces.Services
{
    public interface INotificationService
    {
        Task<BaseResponse<bool>> SendAccountRegistrationSMSOtpAsync(AccountCreationSMSOtp otp);
        Task SendEmailAsync(string to, string subject, string htmlContent);
        Task SendSmsAsync(string phoneNumber, string message);
        Task SendResetPasswordEmail(string email, string resetUrl);
    }
}
