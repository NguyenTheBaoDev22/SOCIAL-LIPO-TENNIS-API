using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Loại bỏ các ký tự đặc biệt như xuống dòng, tab khỏi chuỗi
        /// </summary>
        public static string CleanJsonString(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return input.Replace("\r", "")
                        .Replace("\n", "")
                        .Replace("\t", "")
                        .Trim();
        }

        /// <summary>
        /// Trích xuất và làm sạch trường "payload" từ chuỗi JSON
        /// </summary>
        public static string CleanJsonPayload(string rawJson)
        {
            if (string.IsNullOrEmpty(rawJson)) return rawJson;

            var jsonObject = JObject.Parse(rawJson);
            if (jsonObject.ContainsKey("payload"))
            {
                var payload = jsonObject["payload"]?.ToString();
                return CleanJsonString(payload);
            }

            return rawJson;
        }

        public static string ToLowerCase(string input) => input?.ToLower();
        public static string ToUpperCase(string input) => input?.ToUpper();

        /// <summary>
        /// Chuẩn hóa tên: loại khoảng trắng thừa
        /// </summary>
        public static string NormalizeName(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : input.Trim();
        }

        /// <summary>
        /// Chuyển chuỗi thành dạng slug-friendly dùng trong URL
        /// </summary>
        public static string Slugify(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            input = RemoveVietnameseAccent(input.ToLowerInvariant());

            // Thay các cụm ký tự không hợp lệ thành dấu "-"
            input = Regex.Replace(input, @"[^a-z0-9]+", "-");

            // Gộp dấu '-' liên tiếp và loại bỏ ở đầu/cuối
            input = Regex.Replace(input, @"-+", "-").Trim('-');

            return input;
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt khỏi chuỗi, hỗ trợ cả in hoa và in thường
        /// </summary>
        public static string RemoveVietnameseAccent(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = input.ToLowerInvariant(); // đảm bảo đồng bộ key dictionary

            var sb = new StringBuilder();
            var vietnameseAccents = new Dictionary<char, char>
            {
                {'á', 'a'}, {'à', 'a'}, {'ả', 'a'}, {'ã', 'a'}, {'ạ', 'a'}, {'ă', 'a'}, {'ắ', 'a'},
                {'ằ', 'a'}, {'ẳ', 'a'}, {'ẵ', 'a'}, {'ặ', 'a'}, {'â', 'a'}, {'ấ', 'a'}, {'ầ', 'a'},
                {'ẩ', 'a'}, {'ẫ', 'a'}, {'ậ', 'a'}, {'é', 'e'}, {'è', 'e'}, {'ẻ', 'e'}, {'ẽ', 'e'},
                {'ẹ', 'e'}, {'ê', 'e'}, {'ế', 'e'}, {'ề', 'e'}, {'ể', 'e'}, {'ễ', 'e'}, {'ệ', 'e'},
                {'í', 'i'}, {'ì', 'i'}, {'ỉ', 'i'}, {'ĩ', 'i'}, {'ị', 'i'}, {'ó', 'o'}, {'ò', 'o'},
                {'ỏ', 'o'}, {'õ', 'o'}, {'ọ', 'o'}, {'ô', 'o'}, {'ố', 'o'}, {'ồ', 'o'}, {'ổ', 'o'},
                {'ỗ', 'o'}, {'ộ', 'o'}, {'ơ', 'o'}, {'ớ', 'o'}, {'ờ', 'o'}, {'ở', 'o'}, {'ỡ', 'o'},
                {'ợ', 'o'}, {'ú', 'u'}, {'ù', 'u'}, {'ủ', 'u'}, {'ũ', 'u'}, {'ụ', 'u'}, {'ư', 'u'},
                {'ứ', 'u'}, {'ừ', 'u'}, {'ử', 'u'}, {'ữ', 'u'}, {'ự', 'u'}, {'ý', 'y'}, {'ỳ', 'y'},
                {'ỷ', 'y'}, {'ỹ', 'y'}, {'ỵ', 'y'}, {'đ', 'd'}
            };

            foreach (var c in input)
            {
                sb.Append(vietnameseAccents.TryGetValue(c, out var noAccent) ? noAccent : c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Chuẩn hóa chuỗi dùng làm mã code: loại dấu, lowercase, chỉ còn a-z và 0-9
        /// </summary>
        public static string NormalizeCode(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = input.Trim();
            input = RemoveVietnameseDiacritics(input);
            input = input.ToLowerInvariant();
            input = Regex.Replace(input, @"[^a-z0-9\-_.]", ""); // chỉ giữ lại a-z, 0-9, -, _, .

            return input;
        }

        public static string RemoveVietnameseDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var ch in formD)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        /// <summary>
        /// Kiểm tra chuỗi email có hợp lệ không
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            try
            {
                var emailCheck = new System.Net.Mail.MailAddress(email);
                return emailCheck.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra định dạng số điện thoại Việt Nam (+84xxxxxxxxx)
        /// </summary>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;

            return Regex.IsMatch(phoneNumber, @"^\+84\d{9,10}$");
        }
    }
}
