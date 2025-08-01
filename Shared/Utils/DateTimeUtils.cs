using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class DateTimeUtils
    {
        public static long ToUnixTimestamp(DateTime dateTime)
        {
            var utc = dateTime.ToUniversalTime();
            var unixEpoch = new DateTime(1970, 1, 1);
            return (long)(utc - unixEpoch).TotalSeconds;
        }

        public static string ToIso8601(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
        }

        public static string Format(DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format, CultureInfo.InvariantCulture);
        }

        public static DateTime FromUnixTimestamp(long timestamp)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return unixEpoch.AddSeconds(timestamp);
        }
    }
}
