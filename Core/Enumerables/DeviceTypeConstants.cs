namespace Core.Enumerables
{
    /// <summary>
    /// Loại thiết bị thanh toán (POS, Soundbox, Tablet, ...)
    /// </summary>
    public static class DeviceTypeConstants
    {
        //public const string POS = "POS";
        public const string Soundbox = "Soundbox";
        //public const string Tablet = "Tablet";
        //public const string QRStandee = "QR Standee";
        //public const string MobilePOS = "Mobile POS";

        // Mapping từ mã → tên thiết bị (nếu cần phân biệt hiển thị)
        private static readonly Dictionary<string, string> CodeToName = new()
        {
           // [POS] = "Máy POS cố định",
            [Soundbox] = "Loa đọc thanh toán",
            //[Tablet] = "Tablet bán hàng",
            //[QRStandee] = "Mã QR in bảng",
            //[MobilePOS] = "POS di động"
        };

        /// <summary>
        /// Danh sách các loại thiết bị hợp lệ.
        /// </summary>
        public static IReadOnlyCollection<string> All => CodeToName.Keys;

        /// <summary>
        /// Kiểm tra thiết bị có hợp lệ không.
        /// </summary>
        public static bool IsValid(string deviceType)
        {
            return !string.IsNullOrWhiteSpace(deviceType)
                   && All != null
                   && All.Contains(deviceType);
        }

        /// <summary>
        /// Lấy tên hiển thị theo loại thiết bị.
        /// </summary>
        public static string? GetName(string code)
        {
            return CodeToName.TryGetValue(code, out var name) ? name : null;
        }
        public static IReadOnlyDictionary<string, string> GetAllWithNames()
        {
            return CodeToName;
        }
    }
}
