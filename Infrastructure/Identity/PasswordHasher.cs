using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Identity
{
    /// <summary>
    /// Implementation của IPasswordHasher dùng thư viện BCrypt để hash và verify mật khẩu.
    /// Đặt ở tầng Infrastructure vì phụ thuộc thư viện bên ngoài.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int _workFactor;

        public PasswordHasher(IConfiguration configuration)
        {
            _workFactor = configuration.GetValue<int>("Security:PasswordHashing:WorkFactor", 12); // mặc định 12
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: _workFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
