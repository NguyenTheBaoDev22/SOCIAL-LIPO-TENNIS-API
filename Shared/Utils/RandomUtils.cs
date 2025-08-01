using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class RandomUtils
    {
        private static readonly string Base36Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string GenerateRandomCode(int length = 8)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            var result = new StringBuilder(length);

            foreach (var b in bytes)
            {
                result.Append(Base36Chars[b % Base36Chars.Length]);
            }

            return result.ToString();
        }

        public static string GetBase36FromNow()
        {
            var now = DateTime.UtcNow;
            var timestamp = (long)(now - new DateTime(2024, 1, 1)).TotalMilliseconds;
            return ToBase36(timestamp);
        }

        public static string ToBase36(long value)
        {
            var sb = new StringBuilder();
            while (value > 0)
            {
                sb.Insert(0, Base36Chars[(int)(value % 36)]);
                value /= 36;
            }
            return sb.ToString().PadLeft(9, '0');
        }

        public static string GetSha256Hash(string input)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
