namespace Core.Enumerables
{
    /// <summary>
    /// Trạng thái hoạt động của MerchantBranch được trả về cho Partner
    /// </summary>
    public static class EBranchStatus
    {
        public const string Active = "Active";                            // Đang hoạt động bình thường
        public const string Inactive = "Inactive";                        // Không hoạt động (tạm ngưng thủ công)
        public const string TemporarilyClosed = "TemporarilyClosed";      // Đóng cửa tạm thời
        public const string PermanentlyClosed = "PermanentlyClosed";      // Đã đóng vĩnh viễn

        /// <summary>
        /// Danh sách tất cả trạng thái hợp lệ
        /// </summary>
        public static readonly string[] All = new[]
        {
        Active,
        Inactive,
        TemporarilyClosed,
        PermanentlyClosed
    };
    }

}
