using Core.Entities.AppUsers;

namespace Applications.Services.Interfaces
{
    /// <summary>
    /// Service xử lý logic người dùng hệ thống.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Lấy người dùng theo username hoặc email.
        /// </summary>
        Task<User?> GetByUsernameOrEmailAsync(string input);

        /// <summary>
        /// Lấy người dùng theo ID.
        /// </summary>
        Task<User?> GetByIdAsync(Guid userId);

        /// <summary>
        /// Kiểm tra tồn tại theo username hoặc email.
        /// </summary>
        Task<bool> ExistsByUsernameOrEmailAsync(string input);

        /// <summary>
        /// Kiểm tra số điện thoại hoặc email đã tồn tại.
        /// </summary>
        Task<bool> ExistsByPhoneOrEmailAsync(string phone, string? email = null);

        /// <summary>
        /// Tạo người dùng mới, hash password và set trạng thái.
        /// </summary>
        Task<User> CreateUserAsync(User user, string passwordPlainText, bool isVerified = false, bool isActive = true);

        /// <summary>
        /// Gán role cho user trong một tenant/merchant/branch cụ thể.
        /// </summary>
        Task AssignRoleAsync(User user, Guid roleId, Guid tenantId, Guid merchantId, Guid? merchantBranchId = null);
    }
}
