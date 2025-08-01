using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities.AppUsers;
using Core.Enumerables;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Applications.Services.Implementations
{
    /// <summary>
    /// Triển khai các xử lý OTP: tạo, lưu, xác minh.
    /// </summary>
    public class OtpService : IOtpService
    {
        private readonly IOtpCodeLogRepository _otpRepo;

        public OtpService(IOtpCodeLogRepository otpRepo)
        {
            _otpRepo = otpRepo;
        }

        /// <summary>
        /// Tạo mã OTP ngẫu nhiên với độ dài chỉ định.
        /// </summary>
        public string GenerateOtp(int length)
        {
            var random = new Random();
            return string.Concat(Enumerable.Range(0, length).Select(_ => random.Next(0, 10)));
        }

        /// <summary>
        /// Lưu OTP cho quên mật khẩu
        /// </summary>
        public async Task SaveForgotPasswordOtpAsync(string phoneNumber, string otpCode, TimeSpan ttl)
        {
            await SaveOtpInternalAsync(phoneNumber, otpCode, ttl, OtpPurpose.ForgotPassword);
        }

        /// <summary>
        /// Lưu OTP cho đăng nhập
        /// </summary>
        public async Task SaveLoginOtpAsync(string phoneNumber, string otpCode, TimeSpan ttl)
        {
            await SaveOtpInternalAsync(phoneNumber, otpCode, ttl, OtpPurpose.Login);
        }

        /// <summary>
        /// Lưu OTP cho đăng ký
        /// </summary>
        public async Task SaveRegisterOtpAsync(string phoneNumber, string otpCode, TimeSpan ttl)
        {
            await SaveOtpInternalAsync(phoneNumber, otpCode, ttl, OtpPurpose.Register);
        }

        /// <summary>
        /// Lưu OTP cho cập nhật số điện thoại
        /// </summary>
        public async Task SaveUpdatePhoneOtpAsync(string phoneNumber, string otpCode, TimeSpan ttl)
        {
            await SaveOtpInternalAsync(phoneNumber, otpCode, ttl, OtpPurpose.UpdatePhone);
        }

        /// <summary>
        /// Thực thi lưu OTP nội bộ.
        /// </summary>
        private async Task SaveOtpInternalAsync(string phoneNumber, string otpCode, TimeSpan ttl, string purpose)
        {
            var now = DateTime.UtcNow;
            var entity = new OtpCodeLog
            {
                Id = Guid.NewGuid(),
                PhoneNumber = phoneNumber,
                OtpCode = otpCode,
                Purpose = purpose,
                ExpireAt = now.Add(ttl),
                CreatedAt = now,
                IsVerified = false
            };
            await _otpRepo.AddAsync(entity);
        }

        /// <summary>
        /// Xác minh OTP đúng, chưa xác minh và chưa hết hạn. Ưu tiên OTP mới nhất.
        /// </summary>
        public async Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode, string purpose)
        {
            var now = DateTime.UtcNow;

            var records = await _otpRepo.FindAllAsync(x =>
                x.PhoneNumber == phoneNumber &&
                x.OtpCode == otpCode &&
                x.Purpose == purpose &&
                !x.IsVerified &&
                x.ExpireAt >= now);

            var latest = records.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
            if (latest == null) return false;

            latest.IsVerified = true;
            await _otpRepo.UpdateAsync(latest);
            return true;
        }
    }
}
