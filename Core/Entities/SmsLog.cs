namespace Core.Entities
{
    // ✅ 1. Entity: SmsLog
    public class SmsLog : Audit
    {
        public string Phone { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string UnicodeType { get; set; } = null!;
        public string StatusCode { get; set; } = null!;
        public string StatusDetail { get; set; } = null!;
        public string? TransactionId { get; set; }
        public string? RequestPayload { get; set; }
        public string? RawResponse { get; set; }
        public long ResponseTimeMs { get; set; }
        public string UseCase { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public string SendSMSPath { get; set; } = null!;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public int RetryCount { get; set; } = 0;
        public DateTime? LastRetryAt { get; set; }
        public bool IsSuccess => StatusCode == "0";
        public string Provider { get; set; } = "CÔNG TY CỔ PHẦN VIỄN THÔNG DI ĐỘNG";
    }
}
