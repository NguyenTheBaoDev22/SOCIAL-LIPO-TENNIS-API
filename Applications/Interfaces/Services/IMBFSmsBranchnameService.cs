using Applications.DTOs;
using Shared.Results;

namespace Applications.Interfaces.Services
{
    public interface IMBFSmsBranchnameService
    {
        Task<BaseResponse<bool>> SendUnicodeSmsAsync(string phone, string message); // Gửi SMS Unicode
        Task<BaseResponse<bool>> SendNonUnicodeSmsAsync(string phone, string message); // Gửi SMS Non-Unicode
        Task<BaseResponse<bool>> SendSmsInternalAsync(string phone, string message, string unicode, bool isRetry = false, string useCase = "AccountRegistration"); // Gửi SMS nội bộ
        Task<BaseResponse<bool>> SendAccountRegistrationUnicodeSMSOtpAsync(AccountCreationSMSOtp req); // Gửi OTP đăng ký tài khoản
        Task<BaseResponse<bool>> SendAccountCreationUnicodeSMSConfirmationAsync(AccountCreationSMSConfirmation req); // Gửi thông báo xác nhận tài khoản
        Task<BaseResponse<bool>> SendAccountRegistrationNonUnicodeSMSOtpAsync(AccountCreationSMSOtp req); // Gửi OTP đăng ký tài khoản
        Task<BaseResponse<bool>> SendAccountCreationNonUnicodeSMSConfirmationAsync(AccountCreationSMSConfirmation req); // Gửi thông báo xác nhận tài khoản
    }
}
