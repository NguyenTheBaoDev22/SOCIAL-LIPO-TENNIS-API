namespace Core.Enumerables
{
    /// <summary>
    /// Danh sách trạng thái người dùng hệ thống, định nghĩa dưới dạng hằng số chuỗi.
    /// Dùng thay cho enum khi cần lưu chuỗi vào database hoặc tương tác với frontend.
    /// </summary>
    public static class UserStatusesEnum
    {
        /// <summary>
        /// Người dùng đang hoạt động bình thường.
        /// </summary>
        public const string Active = "Active";

        /// <summary>
        /// Người dùng bị vô hiệu hóa (tạm dừng sử dụng).
        /// </summary>
        public const string Inactive = "Inactive";

        /// <summary>
        /// Người dùng bị khóa (do vi phạm hoặc bảo mật).
        /// </summary>
        public const string Locked = "Locked";

        /// <summary>
        /// Mảng tập hợp tất cả trạng thái hợp lệ.
        /// Hữu ích khi cần kiểm tra hợp lệ (validation).
        /// </summary>
        public static readonly string[] All = { Active, Inactive, Locked };
    }
}
