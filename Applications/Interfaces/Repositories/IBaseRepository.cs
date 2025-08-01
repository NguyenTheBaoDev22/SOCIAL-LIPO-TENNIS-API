using Core;
using Shared.Results;
using Shared.Specifications;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories
{
    //public interface IBaseRepository<T> where T : Audit
    //{
    //    // Read operations
    //    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    //    Task<T?> GetById(Guid id, CancellationToken cancellationToken = default); // Use for synchronous fetch
    //    IQueryable<T> GetAll();  // Returns all non-deleted items as IQueryable
    //    IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null); // Supports dynamic querying with predicate
    //    IQueryable<T> Find(Expression<Func<T, bool>> predicate); // Supports lazy loading with predicates

    //    // Add operations
    //    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    //    Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    //    // Update operations
    //    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    //    void Update(T entity);

    //    // Delete operation (soft delete)
    //    void Delete(T entity);

    //    // Check operations
    //    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    //    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    //    // Pagination support
    //    Task<BaseResponse<PaginatedResult<T>>> GetAllPaginatedAsync(
    //        int pageIndex = 1,
    //        int pageSize = 10,
    //        Expression<Func<T, bool>>? filter = null,
    //        string? sortField = null,
    //        string? sortDirection = "asc",
    //        CancellationToken cancellationToken = default);

    //    // Find by specific code
    //    Task<T?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);

    //    // Soft delete and querying for entities with support for flexibility
    //    IQueryable<T> AsQueryable();

    //    // Save changes to the database (useful when UnitOfWork is not implemented)
    //    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

    //    // Specification pattern for querying
    //    Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    //    IQueryable<T> List(ISpecification<T> specification);

    //    // For finding entities asynchronously by a predicate with flexible type
    //    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    //}

    /// <summary>
    /// Interface cơ bản cho repository xử lý entity chung.
    /// </summary>
    /// <typeparam name="T">Entity class</typeparam>
    public interface IBaseRepository<T> where T : Audit
    {
        IQueryable<T> Query();
        IQueryable<T> Query(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T?> GetById(Guid id, CancellationToken cancellationToken = default);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        IQueryable<T> GetAll();
        Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        void Delete(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        IQueryable<T> AsQueryable();
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<BaseResponse<PaginatedResult<T>>> GetAllPaginatedAsync(
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<T, bool>>? filter = null,
            string? sortField = null,
            string? sortDirection = "asc",
            CancellationToken cancellationToken = default);

        Task<BaseResponse<PaginatedResult<TDto>>> GetAllPaginatedAsync<TDto>(
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<T, bool>>? filter = null,
            string? sortField = null,
            string? sortDirection = "asc",
            CancellationToken cancellationToken = default);

        Task<T?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null);
        IQueryable<T> List(ISpecification<T> specification);
        Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<bool> AnyByTenantAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllByTenantAsync(CancellationToken cancellationToken = default);
    }
}
