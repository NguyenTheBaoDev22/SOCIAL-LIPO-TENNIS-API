namespace Shared.Results
{
    /// <summary>
    /// Metadata cho phân trang
    /// </summary>
    public class PaginationMeta
    {
        public int CurrentPage { get; set; }  // Trang hiện tại
        public int PageSize { get; set; }     // Số mục mỗi trang
        public int TotalPages { get; set; }   // Tổng số trang
        public int TotalItems { get; set; }   // Tổng số mục
        public bool HasPrevious { get; set; } // Có trang trước không
        public bool HasNext { get; set; }     // Có trang tiếp theo không
    }
}
