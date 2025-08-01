namespace Core.Entities
{
    // ✅ 2. Entity: SmsRetryQueue
    public class SmsRetryQueue : Audit
    {
        public Guid SmsLogId { get; set; }
        public SmsLog SmsLog { get; set; } = null!;
        public int RetryCount { get; set; } = 0;
        public DateTime? LastTriedAt { get; set; }
        public bool IsDone { get; set; } = false;
    }
}
