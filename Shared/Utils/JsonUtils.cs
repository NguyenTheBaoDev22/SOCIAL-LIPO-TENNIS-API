using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class JsonUtils
    {
        public static string CleanJsonString(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return string.Empty;

            // Xóa xuống dòng, tab, khoảng trắng dư thừa giữa các field
            json = Regex.Replace(json, @"\r\n?|\n|\t", "");
            json = Regex.Replace(json, @"\s{2,}", " ");

            return json.Trim();
        }

        public static string CleanJsonPayload(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return string.Empty;

            var cleaned = CleanJsonString(json);
            cleaned = cleaned.Replace("\\", ""); // remove escape chars
            return cleaned;
        }
    }
}
