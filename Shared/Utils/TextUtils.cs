using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class TextUtils
    {
        public static string RemoveVietnameseAccent(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var normalized = input.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemoveSpecialCharacters(string input, string allowed = "")
        {
            return Regex.Replace(input, $"[^a-zA-Z0-9{Regex.Escape(allowed)}]+", "", RegexOptions.Compiled);
        }

        public static string NormalizeCode(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var text = RemoveVietnameseAccent(input);
            text = Regex.Replace(text, @"\s+", ""); // xóa khoảng trắng
            return text.ToLowerInvariant();
        }

        public static string Slugify(string input, string separator = "-")
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var slug = RemoveVietnameseAccent(input);
            slug = slug.ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", ""); // giữ chữ, số, khoảng trắng và -
            slug = Regex.Replace(slug, @"\s+", separator); // thay khoảng trắng bằng separator
            slug = Regex.Replace(slug, $"{Regex.Escape(separator)}+", separator); // gộp các separator liền nhau
            return slug.Trim(separator.ToCharArray());
        }

        public static string NormalizeName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var text = RemoveVietnameseAccent(input);
            text = Regex.Replace(text, @"\s+", ""); // bỏ khoảng trắng
            return text.ToLowerInvariant();
        }
    }
}
