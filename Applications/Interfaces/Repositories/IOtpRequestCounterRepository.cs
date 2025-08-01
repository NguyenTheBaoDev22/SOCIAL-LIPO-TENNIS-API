using Core.Entities;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories
{
    /// <summary>
    /// Giao diện repository theo dõi và giới hạn số lần gửi OTP.
    /// </summary>
    public interface IOtpRequestCounterRepository : IBaseRepository<OtpRequestCounter>
    {
        /// <summary>
        /// Lấy bộ đếm OTP theo số điện thoại và mục đích gửi.
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <param name="purpose">Mục đích gửi OTP</param>
        /// <returns>Thông tin bộ đếm hoặc null nếu không tồn tại</returns>
        Task<OtpRequestCounter?> GetAsync(string phoneNumber, string purpose, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tăng bộ đếm nếu chưa vượt giới hạn trong 24h.
        /// Nếu vượt giới hạn thì trả về false.
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <param name="purpose">Mục đích gửi OTP</param>
        /// <param name="maxPerDay">Số lần tối đa trong 24h (default = 3)</param>
        /// <returns>True nếu tăng thành công, false nếu đã vượt giới hạn</returns>
        Task<bool> TryIncreaseCountAsync(string phoneNumber, string purpose, int maxPerDay = 3, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reset thủ công bộ đếm OTP về 0. Chỉ dành cho Admin.
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <param name="purpose">Mục đích gửi OTP</param>
        /// <returns>Kết quả thành công hoặc thất bại</returns>
        Task<BaseResponse<bool>> ResetCounterAsync(string phoneNumber, string purpose, CancellationToken cancellationToken = default);

        /// <summary>
        /// Thêm mới bộ đếm hoặc cập nhật nếu đã tồn tại.
        /// </summary>
        /// <param name="entity">Thông tin bộ đếm cần thêm hoặc cập nhật</param>
        Task AddOrUpdateAsync(OtpRequestCounter entity, CancellationToken cancellationToken = default);
    }
}
