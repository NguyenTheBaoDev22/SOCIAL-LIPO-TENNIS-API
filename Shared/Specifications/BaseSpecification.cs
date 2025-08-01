using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Shared.Specifications
{
    /// <summary>
    /// Lớp cơ sở cho Specification Pattern. Hỗ trợ WHERE, INCLUDE, ORDERBY, PAGING.
    /// </summary>
    /// <typeparam name="T">Entity áp dụng specification</typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class  // Added the 'where T : class' constraint here
    {
        public Expression<Func<T, bool>>? Criteria { get; protected set; }

        public List<Expression<Func<T, object>>> Includes { get; } = new();
        private readonly List<string> _includeStrings = new();
        public IEnumerable<string> IncludeStrings => _includeStrings;

        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderByExpression { get; private set; }

        public int? Skip { get; private set; }
        public int? Take { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        /// <summary>
        /// Constructor mặc định.
        /// </summary>
        protected BaseSpecification() { }

        /// <summary>
        /// Constructor có điều kiện lọc.
        /// </summary>
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Thêm include bằng biểu thức lambda.
        /// </summary>
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Thêm include bằng tên chuỗi (dùng khi không thể dùng expression).
        /// </summary>
        protected void AddInclude(string includeString)
        {
            _includeStrings.Add(includeString);
        }

        /// <summary>
        /// Thiết lập biểu thức sắp xếp (type-safe).
        /// </summary>
        protected void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression)
        {
            OrderByExpression = orderByExpression;
        }

        /// <summary>
        /// Áp dụng phân trang với skip/take.
        /// </summary>
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        // Implement the Apply method
        public IQueryable<T> Apply(IQueryable<T> query)
        {
            // Apply filtering criteria if available
            if (Criteria != null)
            {
                query = query.Where(Criteria);
            }

            // Apply include expressions if available
            foreach (var include in Includes)
            {
                // Include method available in EF Core
                query = query.Include(include);
            }

            // Apply sorting if available
            if (OrderByExpression != null)
            {
                query = OrderByExpression(query);
            }

            // Apply pagination if enabled
            if (IsPagingEnabled)
            {
                query = query.Skip(Skip ?? 0).Take(Take ?? 10);
            }

            return query;
        }
    }
}
