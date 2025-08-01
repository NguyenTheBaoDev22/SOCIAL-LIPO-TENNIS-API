using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    /// <summary>
    /// Audit base class, dùng cho toàn bộ entity có audit (Created/Updated/Delete)
    /// </summary>
    public abstract class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Người tạo (UUID của tài khoản), có thể là hệ thống
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Thời gian tạo (UTC, ISO 8601)
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Người cập nhật gần nhất
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Thời gian cập nhật gần nhất (UTC)
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Đánh dấu xóa mềm (chỉ ẩn khỏi truy vấn, không xóa vật lý)
        /// </summary>
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        /// <summary>
        /// IP của người thao tác (nếu cần trace)
        /// </summary>
        [MaxLength(45)] // IPv6
        public string? IpAddress { get; set; }

        /// <summary>
        /// Trace ID hoặc Request ID (gắn từ middleware để trace)
        /// </summary>
        [MaxLength(100)]
        public string? TraceId { get; set; }
    }
}
