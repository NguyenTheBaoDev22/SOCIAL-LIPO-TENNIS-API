namespace Core.Enumerables
{
    public static class RoleEnum
    {
        public const string MobileShopManager = "MobileShopManager"; // App quản lý shop
        public const string RetailPOS = "RetailPOS";                 // POS station
        public const string ZenPayCore = "ZenPayCore";               // Core system gọi QR
        public const string AdminDashboard = "AdminDashboard";       // Admin site gọi
        public const string Unknown = "Unknown";
        /// <summary>
        /// Mảng tập hợp tất cả các vai trò hợp lệ.
        /// Hữu ích khi cần kiểm tra vai trò hợp lệ (validation).
        /// </summary>
        public static readonly string[] All = { MobileShopManager, RetailPOS, ZenPayCore, AdminDashboard, Unknown };
    }

}
