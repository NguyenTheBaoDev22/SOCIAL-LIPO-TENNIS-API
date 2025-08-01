namespace Shared.Results
{
    /// <summary>
    /// Gói trả về kèm dữ liệu phân trang
    /// </summary>
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();  // Danh sách các mục trong trang hiện tại
        public PaginationMeta Meta { get; set; } = new();  // Thông tin phân trang

        // Phương thức tạo PaginatedResult từ danh sách các mục, pageIndex, pageSize và tổng số mục
        public static PaginatedResult<T> Create(IEnumerable<T> items, int pageIndex, int pageSize, int totalCount)
        {
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);  // Tính toán tổng số trang

            // Tạo và trả về PaginatedResult
            return new PaginatedResult<T>
            {
                Items = items,
                Meta = new PaginationMeta
                {
                    CurrentPage = pageIndex,      // Trang hiện tại
                    PageSize = pageSize,          // Số mục mỗi trang
                    TotalItems = totalCount,      // Tổng số mục
                    TotalPages = totalPages,      // Tổng số trang
                    HasPrevious = pageIndex > 1,  // Có trang trước không
                    HasNext = pageIndex < totalPages  // Có trang tiếp theo không
                }
            };
        }
    }
}
