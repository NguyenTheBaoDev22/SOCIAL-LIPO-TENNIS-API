using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Shared.Results
{
    /// <summary>
    /// Generic response wrapper dùng cho toàn bộ API.
    /// Gói dữ liệu trả về, trạng thái, thông điệp, traceId và thời gian tạo.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu trả về trong response</typeparam>
    public class BaseResponse<T>
    {
        /// <summary>
        /// Mã trạng thái phản hồi (ví dụ: "00" = thành công, "400" = lỗi hệ thống,...)
        /// </summary>
        public string Code { get; set; } = "00";  // Mã trạng thái mặc định là "00" (Success)

        /// <summary>
        /// Thông điệp mô tả trạng thái phản hồi.
        /// </summary>
        public string Message { get; set; } = "Success";  // Mặc định là "Success"

        /// <summary>
        /// Dữ liệu trả về (nếu có)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// TraceId dùng để theo dõi log cho request này, hỗ trợ Debug/Trace.
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// Dấu thời gian phản hồi được tạo ra (UTC). Mặc định là thời gian hiện tại.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Kiểm tra phản hồi có thành công hay không (Code == "00")
        /// </summary>
        public bool IsSuccess => Code == "00";  // Kiểm tra trạng thái thành công (Code == "00")

        /// <summary>
        /// Tạo phản hồi thành công với dữ liệu
        /// </summary>
        public static BaseResponse<T> Success(T data, string message = "Success", string code = "00")
        {
            // Trả về một phản hồi thành công với dữ liệu, thông điệp và mã trạng thái mặc định
            return new()
            {
                Code = code,
                Message = message,
                Data = data,
                TraceId = Activity.Current?.Id,  // Lấy TraceId từ Activity nếu có
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Tạo phản hồi lỗi với thông điệp và mã lỗi
        /// </summary>
        public static BaseResponse<T> Error(string message, string code = "400")
        {
            // Trả về phản hồi lỗi với thông điệp và mã lỗi mặc định là "400"
            return new()
            {
                Code = code,
                Message = message,
                Data = default!,  // Không có data, set mặc định là null
                TraceId = Activity.Current?.Id,  // Lấy TraceId từ Activity nếu có
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Tạo phản hồi lỗi với TraceId thủ công
        /// </summary>
        public static BaseResponse<T> Error(string message, string code, string traceId)
        {
            // Trả về phản hồi lỗi với TraceId thủ công
            return new()
            {
                Code = code,
                Message = message,
                Data = default!,  // Không có data
                TraceId = traceId,  // Sử dụng TraceId thủ công
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Phản hồi phân trang (Pagination) với dữ liệu dạng PaginatedResult.
        /// </summary>
        public static BaseResponse<PaginatedResult<T>> SuccessPaginated(
            IQueryable<T> query,
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            string? search = null,
            string[]? searchFields = null,
            string? sortField = null,
            string? sortDirection = "asc")
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;  // Đảm bảo pageIndex không nhỏ hơn 1
            pageSize = pageSize <= 0 ? 10 : pageSize;   // Đảm bảo pageSize không nhỏ hơn 1

            // Áp dụng filter nếu có
            if (filter != null)
                query = query.Where(filter);

            // Sử dụng Dynamic LINQ để sắp xếp nếu có sortField
            if (!string.IsNullOrEmpty(sortField))
            {
                var sortExpression = $"{sortField} {sortDirection}";  // Cấu trúc chuỗi sắp xếp
                query = query.OrderBy(sortExpression);  // Dynamic LINQ OrderBy
            }

            var totalCount = query.Count();  // Tính tổng số bản ghi
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();  // Phân trang dữ liệu

            // Tạo PaginatedResult
            var paginatedResult = PaginatedResult<T>.Create(items, pageIndex, pageSize, totalCount);

            // Trả về phản hồi thành công với kết quả phân trang
            return new BaseResponse<PaginatedResult<T>>()
            {
                Code = "00",
                Message = "Success",
                Data = paginatedResult,
                TraceId = Activity.Current?.Id,  // Lấy TraceId từ Activity nếu có
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Tạo phản hồi từ BusinessException để dễ dàng chuyển đổi từ lỗi business
        /// </summary>
        public static BaseResponse<T> FromBusinessException(Shared.Exceptions.BusinessException ex)
        {
            return new()
            {
                Code = ex.Code ?? "400",  // Dùng Code từ exception, nếu không có thì mặc định là "400"
                Message = ex.Message,
                Data = default!,  // Không có data
                TraceId = ex.TraceId ?? Activity.Current?.Id,  // Lấy TraceId từ exception nếu có
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Tạo phản hồi lỗi từ exception
        /// </summary>
        public static BaseResponse<T> Fail(Exception ex, string code = "9901")
        {
            return new()
            {
                Code = code,  // Dùng mã lỗi hệ thống mặc định
                Message = ex.Message,
                Data = default!,  // Không có data
                TraceId = Activity.Current?.Id,  // Lấy TraceId từ Activity nếu có
                Timestamp = DateTime.UtcNow
            };
        }
        /// <summary>
        /// Phản hồi dạng phân trang khi truyền vào IEnumerable + meta info (có sẵn)
        /// </summary>
        public static BaseResponse<PaginatedResult<T>> SuccessPaginated(
            IEnumerable<T> items,
            int pageIndex,
            int pageSize,
            int totalCount)
        {
            var paginatedResult = PaginatedResult<T>.Create(items, pageIndex, pageSize, totalCount);

            return new BaseResponse<PaginatedResult<T>>
            {
                Code = "00",
                Message = "Success",
                Data = paginatedResult,
                TraceId = Activity.Current?.Id,
                Timestamp = DateTime.UtcNow
            };
        }
        /// <summary>
        /// Danh sách lỗi chi tiết theo field (thường dùng cho validation, binding)
        /// </summary>
        ///  Bổ sung JsonIgnore nếu bạn không muốn Errors hiện trong response khi không có lỗi:
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string[]>? Errors { get; set; } = new();
    }

}
