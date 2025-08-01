using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    /// <summary>
    /// Cung cấp các hàm hỗ trợ tạo và xác minh mật khẩu với salt và hash an toàn.
    /// </summary>
    public static class PasswordUtils
    {
        private const int SaltSize = 16; // 128 bit
        private const int HashSize = 32; // 256 bit
        private const int Iterations = 10000; // số lần lặp cho PBKDF2

        /// <summary>
        /// Tạo salt ngẫu nhiên.
        /// </summary>
        public static string GenerateSalt()
        {
            var saltBytes = new byte[SaltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Hash mật khẩu với salt.
        /// </summary>
        public static string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(HashSize);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Xác minh mật khẩu nhập vào có đúng với mật khẩu đã lưu không.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var computedHash = HashPassword(password, storedSalt);
            return SlowEquals(storedHash, computedHash);
        }

        /// <summary>
        /// So sánh 2 chuỗi hash một cách an toàn để tránh timing attack.
        /// </summary>
        private static bool SlowEquals(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);

            uint diff = (uint)aBytes.Length ^ (uint)bBytes.Length;
            for (int i = 0; i < aBytes.Length && i < bBytes.Length; i++)
            {
                diff |= (uint)(aBytes[i] ^ bBytes[i]);
            }

            return diff == 0;
        }
    }
}
