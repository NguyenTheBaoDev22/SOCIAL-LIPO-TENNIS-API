namespace Core.Interfaces
{
    /// <summary>
    /// Interface định nghĩa các phương thức hash và verify mật khẩu.
    /// Đặt ở tầng Core để tách biệt ràng buộc implementation.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Mã hóa mật khẩu thô.
        /// </summary>
        /// <param name="password">Mật khẩu thô</param>
        /// <returns>Mật khẩu đã hash</returns>
        string HashPassword(string password);

        /// <summary>
        /// Kiểm tra mật khẩu thô có khớp với mật khẩu đã hash hay không.
        /// </summary>
        /// <param name="password">Mật khẩu thô</param>
        /// <param name="hashedPassword">Mật khẩu đã hash</param>
        /// <returns>true nếu khớp, ngược lại false</returns>
        bool VerifyPassword(string password, string hashedPassword);
    }
}
