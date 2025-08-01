using System.Linq.Expressions;

namespace Shared.Specifications
{
    /// <summary>
    /// Giao diện Specification Pattern, định nghĩa tiêu chí lọc, bao gồm, sắp xếp và phân trang.
    /// </summary>
    /// <typeparam name="T">Entity áp dụng specification</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Biểu thức điều kiện lọc (WHERE).
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// Danh sách biểu thức bao gồm navigation properties (Include dạng biểu thức).
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Danh sách Include dưới dạng chuỗi (navigation dạng sâu).
        /// </summary>
        IEnumerable<string> IncludeStrings { get; }

        /// <summary>
        /// Biểu thức sắp xếp (OrderBy/ThenBy) an toàn kiểu.
        /// </summary>
        Func<IQueryable<T>, IOrderedQueryable<T>>? OrderByExpression { get; }

        /// <summary>
        /// Số lượng dòng cần bỏ qua (phân trang).
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// Số lượng dòng cần lấy (phân trang).
        /// </summary>
        int? Take { get; }

        /// <summary>
        /// Cờ xác định có sử dụng phân trang hay không.
        /// </summary>
        bool IsPagingEnabled { get; }
        // Apply method for applying filters, sorting, etc.
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
