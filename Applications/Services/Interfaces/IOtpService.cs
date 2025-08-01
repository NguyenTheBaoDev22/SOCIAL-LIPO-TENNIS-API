using Core.Enumerables;
using System;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    /// <summary>
    /// Service xử lý logic liên quan đến OTP.
    /// </summary>
    public interface IOtpService
    {
        /// <summary>
        /// Tạo mã OTP ngẫu nhiên với độ dài chỉ định.
        /// </summary>
        string GenerateOtp(int length);

        /// <summary>
        /// Lưu OTP cho đăng nhập.
        /// </summary>
        Task SaveLoginOtpAsync(string phoneNumber, string otpCode, TimeSpan ttl);

        /// <summary>
        /// Lưu OTP cho quên mật khẩu.
        /// </summary>
        Task SaveForgotPasswordOtpAsync(string phoneNumber, string otpCode, TimeSpan ttl);

        /// <summary>
        /// Lưu OTP cho đăng ký.
        /// </summary>
        Task SaveRegisterOtpAsync(string phoneNumber, string otpCode, TimeSpan ttl);

        /// <summary>
        /// Lưu OTP cho cập nhật số điện thoại.
        /// </summary>
        Task SaveUpdatePhoneOtpAsync(string phoneNumber, string otpCode, TimeSpan ttl);

        /// <summary>
        /// Xác minh OTP đúng và chưa hết hạn.
        /// </summary>
        Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode, string purpose);
    }
}
