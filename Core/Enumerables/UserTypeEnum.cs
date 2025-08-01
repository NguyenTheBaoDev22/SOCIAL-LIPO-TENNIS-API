namespace Core.Enumerables
{
    public static class UserTypeEnum
    {
        // Các loại người dùng
        public const string Individual = "Individual";  // Cá nhân
        public const string Business = "Business";      // Doanh nghiệp

        /// <summary>
        /// Mảng tập hợp tất cả các loại người dùng hợp lệ.
        /// Hữu ích khi cần kiểm tra loại người dùng hợp lệ (validation).
        /// </summary>
        public static readonly string[] All = { Individual, Business };
    }
}
