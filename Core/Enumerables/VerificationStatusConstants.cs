namespace Core.Enumerables
{
    public static class VerificationStatusConstants
    {
        public const string Pending = "Pending";                          // Hồ sơ merchant mới, chưa được thẩm định
        public const string NeedsMoreInfo = "NeedsMoreInfo";
        public const string Approved = "Approved";                        // Merchant đã được thẩm định và chấp nhận
        public const string Rejected = "Rejected";                        // Merchant bị từ chối thẩm định
        public const string ProcessingVerification_1 = "ProcessingVerification_1"; // Đang xử lý thẩm định lần 1
        public const string ProcessingVerification_2 = "ProcessingVerification_2"; // Đang xử lý thẩm định lần 2
        public const string ProcessingVerification_3 = "ProcessingVerification_3"; // Đang xử lý thẩm định lần 3
        public const string VerificationFailed_1 = "VerificationFailed_1"; // Thẩm định lần 1 thất bại
        public const string VerificationFailed_2 = "VerificationFailed_2"; // Thẩm định lần 2 thất bại
        public const string VerificationFailed_3 = "VerificationFailed_3"; // Thẩm định lần 3 thất bại
        public const string Closed = "Closed";                            // Hồ sơ bị đóng sau 3 lần thẩm định không thành công
    }

}
