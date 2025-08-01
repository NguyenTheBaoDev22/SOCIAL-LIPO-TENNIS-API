using Core.Entities.AppUsers;
using Core.Enumerables;
using System.Linq.Expressions;

namespace Applications.Interfaces.Repositories
{
    public interface IOtpCodeLogRepository : IBaseRepository<OtpCodeLog>
    {
        /// <summary>
        /// Lấy mã OTP chưa hết hạn gần nhất theo số điện thoại và mục đích sử dụng.
        /// </summary>
        Task<OtpCodeLog?> GetLatestActiveCodeAsync(string phoneNumber, string purpose, CancellationToken cancellationToken = default);

        /// <summary>
        /// Đánh dấu mã OTP đã sử dụng.
        /// </summary>
        Task MarkAsUsedAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<OtpCodeLog>> FindAllAsync(Expression<Func<OtpCodeLog, bool>> predicate, CancellationToken cancellationToken = default);
        /// <summary>
        /// Lấy mã OTP chưa xác minh gần nhất theo số điện thoại, mã OTP và mục đích gửi.
        /// </summary>
        Task<OtpCodeLog?> GetLatestUnverifiedOtpAsync(string phoneNumber, string otpCode, string purpose, CancellationToken cancellationToken);

        /// <summary>
        /// Đếm số OTP chưa xác thực gửi trong khoảng thời gian theo mục đích
        /// </summary>
        Task<int> CountOtpRequestsInPeriodAsync(string phoneNumber, string purpose, TimeSpan period, CancellationToken cancellationToken = default);

    }
}

