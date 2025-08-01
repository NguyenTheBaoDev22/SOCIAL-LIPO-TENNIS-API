namespace Core.Enumerables
{
    public static class MerchantCallbackStatus
    {
        public const string Pending = "Pending";       // Chờ gửi callback (mới tạo hoặc đang retry)
        public const string Success = "Success";       // Đã gửi callback thành công, merchant xác nhận nhận được
        public const string Failed = "Failed";         // Gửi callback thất bại, cần retry hoặc xử lý lỗi
        public const string Cancelled = "Cancelled";   // Callback bị hủy (ví dụ hủy giao dịch, không cần callback nữa)
        public const string Timeout = "Timeout";       // Callback gửi nhưng merchant không phản hồi kịp thời
        public const string Unknown = "Unknown";       // Trạng thái không xác định / mặc định
    }
}
