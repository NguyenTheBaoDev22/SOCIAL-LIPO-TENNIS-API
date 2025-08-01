using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class ValidationUtils
    {
        public static bool IsValidEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }

        public static bool IsValidPhoneNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            return Regex.IsMatch(input, @"^(0|\+84)(\d{9})$", RegexOptions.IgnoreCase);
        }

        public static bool MatchRegex(string input, string pattern)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            return Regex.IsMatch(input, pattern);
        }
    }
}
