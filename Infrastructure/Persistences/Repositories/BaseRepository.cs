using Applications.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core;
using Core.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Shared.Results;
using Shared.Specifications;
using System;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistences.Repositories
{
    //public class BaseRepository<T> : IBaseRepository<T> where T : Audit
    //{
    //    protected readonly AppDbContext _context;
    //    protected readonly DbSet<T> _dbSet;
    //    private readonly IMapper _mapper;
    //    private readonly ICurrentUserService _currentUser;
    //    public BaseRepository(AppDbContext context, IMapper mapper, ICurrentUserService currentUser)
    //    {
    //        _context = context;
    //        _dbSet = context.Set<T>();
    //        _mapper = mapper;
    //        _currentUser = currentUser;
    //    }
    //    /// <summary>
    //    /// Trả về IQueryable đã tự động lọc theo TenantId nếu T là TenantEntity
    //    /// </summary>
    //    public virtual IQueryable<T> Query()
    //    {
    //        var query = _context.Set<T>().AsQueryable();

    //        // Nếu entity có TenantId thì tự động lọc theo Tenant hiện tại
    //        if (typeof(TenantEntity).IsAssignableFrom(typeof(T)))
    //        {
    //            var tenantId = _currentUser.TenantId ?? Guid.Empty;

    //            // Dùng Expression Tree để build điều kiện where e => e.TenantId == currentTenantId
    //            var param = Expression.Parameter(typeof(T), "e");
    //            var property = Expression.Property(param, nameof(TenantEntity.TenantId));
    //            var constant = Expression.Constant(tenantId);
    //            var condition = Expression.Equal(property, constant);
    //            var lambda = Expression.Lambda<Func<T, bool>>(condition, param);

    //            query = query.Where(lambda);
    //        }

    //        return query;
    //    }

    //    // Optional: Hàm truy vấn có cả điều kiện Where truyền vào
    //    public virtual IQueryable<T> Query(Expression<Func<T, bool>> predicate)
    //    {
    //        return Query().Where(predicate);
    //    }

    //    /// <summary>
    //    /// Cách dùng:
    //    //  Giả sử Product kế thừa TenantEntity, thì:
    //    //var list = await _productRepository.Query()
    //    //    .Where(x => x.CategoryId == id)
    //    //    .ToListAsync(cancellationToken);
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="cancellationToken"></param>
    //    /// <returns></returns>
    //    // Implement GetById method to match IBaseRepository
    //    public async Task<T?> GetById(Guid id, CancellationToken cancellationToken = default)
    //    {
    //        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    //    }

    //    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    //    {
    //        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    //    }

    //    // Implement Find method to match IBaseRepository
    //    public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
    //    {
    //        return _dbSet.Where(x => !x.IsDeleted).Where(predicate);
    //    }

    //    // Cập nhật lại phương thức GetAll để trả về IQueryable<T>
    //    public IQueryable<T> GetAll()  // Phương thức này đã được thêm vào
    //    {
    //        return _dbSet.Where(x => !x.IsDeleted);  // Trả về IQueryable không bị xóa
    //    }

    //    public async Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    //    {
    //        return await Task.FromResult(_dbSet.Where(x => !x.IsDeleted));
    //    }

    //    // Cập nhật FindAsync để trả về Task<IEnumerable<T>>
    //    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    //    {
    //        return await _dbSet.Where(x => !x.IsDeleted).Where(predicate).ToListAsync(cancellationToken);
    //    }

    //    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    //        => await _dbSet.AddAsync(entity, cancellationToken);

    //    public void Update(T entity)
    //        => _dbSet.Update(entity);

    //    public void Delete(T entity)
    //    {
    //        entity.IsDeleted = true;
    //        _dbSet.Update(entity);
    //    }

    //    //public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    //    //    => predicate == null
    //    //        ? await _dbSet.CountAsync(x => !x.IsDeleted, cancellationToken)
    //    //        : await _dbSet.CountAsync(x => !x.IsDeleted && predicate.Compile()(x), cancellationToken);
    //    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    //    {
    //        var query = _dbSet.Where(x => !x.IsDeleted);
    //        if (predicate != null)
    //            query = query.Where(predicate);

    //        return await query.CountAsync(cancellationToken);
    //    }
    //    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    //        => await _dbSet.Where(x => !x.IsDeleted).AnyAsync(predicate, cancellationToken);

    //    public IQueryable<T> AsQueryable()
    //        => _dbSet.Where(x => !x.IsDeleted);

    //    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    //        => await _context.SaveChangesAsync(cancellationToken) > 0;

    //    public async Task<BaseResponse<PaginatedResult<T>>> GetAllPaginatedAsync(
    //        int pageIndex = 1,
    //        int pageSize = 10,
    //        Expression<Func<T, bool>>? filter = null,
    //        string? sortField = null,
    //        string? sortDirection = "asc",
    //        CancellationToken cancellationToken = default)
    //    {
    //        pageSize = pageSize > 100 ? 100 : pageSize;

    //        IQueryable<T> query = _dbSet.Where(x => !x.IsDeleted);

    //        if (filter != null)
    //            query = query.Where(filter);

    //        if (!string.IsNullOrEmpty(sortField))
    //        {
    //            var sortExpression = $"{sortField} {sortDirection}";
    //            query = query.OrderBy(sortExpression);
    //        }

    //        var totalCount = await query.CountAsync(cancellationToken);

    //        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

    //        var mappedItems = _mapper.Map<List<T>>(items);

    //        var paginatedResult = PaginatedResult<T>.Create(mappedItems, pageIndex, pageSize, totalCount);

    //        return BaseResponse<PaginatedResult<T>>.Success(paginatedResult, "List retrieved", "00");
    //    }

    //    public async Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    //    {
    //        await _dbSet.AddRangeAsync(entities, cancellationToken);
    //    }

    //    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    //    {
    //        var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted, cancellationToken);

    //        if (existingEntity == null)
    //        {
    //            throw new KeyNotFoundException($"Entity with Id {entity.Id} not found.");
    //        }

    //        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
    //        await _context.SaveChangesAsync(cancellationToken);
    //    }

    //    public async Task<T?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    //    {
    //        var property = typeof(T).GetProperty("Code");
    //        if (property == null)
    //            throw new InvalidOperationException($"Entity '{typeof(T).Name}' does not have a property named 'Code'.");

    //        return await _dbSet
    //            .AsNoTracking()
    //            .Where(x => !x.IsDeleted)
    //            .FirstOrDefaultAsync(x => EF.Property<string>(x, "Code") == code, cancellationToken);
    //    }

    //    public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null)
    //    {
    //        var query = _context.Set<T>().AsQueryable();
    //        return predicate != null ? query.Where(predicate) : query;
    //    }

    //    public IQueryable<T> List(ISpecification<T> specification)
    //    {
    //        var query = _dbSet.AsQueryable();
    //        if (specification != null)
    //        {
    //            query = specification.Apply(query);
    //        }

    //        return query;
    //    }

    //    public async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    //    {
    //        var query = _dbSet.AsQueryable();
    //        if (specification != null)
    //        {
    //            query = specification.Apply(query);
    //        }

    //        return await query.FirstOrDefaultAsync(cancellationToken);
    //    }
    //    private IQueryable<T> ApplyFilters()
    //    {
    //        var query = _dbSet.AsQueryable();

    //        // Soft delete
    //        query = query.Where(x => !x.IsDeleted);

    //        // Tenant filter nếu T có property TenantId
    //        if (typeof(TenantEntity).IsAssignableFrom(typeof(T)))
    //        {
    //            var tenantId = _currentUser.TenantId;
    //            if (tenantId.HasValue)
    //            {
    //                query = query.Where(e => EF.Property<Guid>(e, "TenantId") == tenantId.Value);
    //            }
    //        }

    //        return query;
    //    }

    //    public async Task<bool> AnyByTenantAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    //    {
    //        var query = _dbSet.AsQueryable();

    //        if (typeof(TenantEntity).IsAssignableFrom(typeof(T)))
    //        {
    //            var tenantId = _currentUser.TenantId ?? throw new InvalidOperationException("TenantId is required but not provided.");
    //            query = query.Where(e => EF.Property<Guid>(e, "TenantId") == tenantId);
    //        }

    //        return await query.AnyAsync(predicate, cancellationToken);
    //    }
    //    public async Task<List<T>> GetAllByTenantAsync(CancellationToken cancellationToken = default)
    //    {
    //        var query = _dbSet.AsQueryable();

    //        if (typeof(TenantEntity).IsAssignableFrom(typeof(T)))
    //        {
    //            var tenantId = _currentUser.TenantId ?? throw new InvalidOperationException("TenantId is required but not provided.");
    //            query = query.Where(e => EF.Property<Guid>(e, "TenantId") == tenantId);
    //        }

    //        return await query.ToListAsync(cancellationToken);
    //    }
    //}



    public class BaseRepository<T> : IBaseRepository<T> where T : Audit
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly IMapper _mapper;
        protected readonly ICurrentUserService _currentUser;
        protected readonly ILogger<BaseRepository<T>> _logger;

        public BaseRepository(AppDbContext context, IMapper mapper, ICurrentUserService currentUser, ILogger<BaseRepository<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
        }

        protected virtual IQueryable<T> BaseQuery()
        {
            var query = _dbSet.AsQueryable().Where(x => !x.IsDeleted);

            if (typeof(TenantEntity).IsAssignableFrom(typeof(T)))
            {
                var tenantId = _currentUser.TenantId ?? throw new InvalidOperationException("TenantId is required.");
                query = query.Where(e => EF.Property<Guid>(e, "TenantId") == tenantId);
            }

            return query;
        }

        public virtual IQueryable<T> Query() => BaseQuery();

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> predicate) => BaseQuery().Where(predicate);

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("🔍 GetByIdAsync: {EntityType} with Id = {Id}", typeof(T).Name, id);
            return await BaseQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<T?> GetById(Guid id, CancellationToken cancellationToken = default)
            => await GetByIdAsync(id, cancellationToken);

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
            => BaseQuery().Where(predicate);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("🔍 FindAsync: {EntityType} with filter: {Predicate}", typeof(T).Name, predicate.ToString());
            return await BaseQuery().Where(predicate).ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetAll() => BaseQuery();

        public Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(BaseQuery());

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            _logger.LogInformation("➕ AddAsync: {EntityType} with Id = {Id} by User {UserId}", typeof(T).Name, entity.Id, _currentUser.UserId);
        }

        public async Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            _logger.LogInformation("➕ AddAsync: {Count} {EntityType} entities added by User {UserId}", entities.Count(), typeof(T).Name, _currentUser.UserId);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _logger.LogInformation("✏️ Update: {EntityType} Id = {Id} by User {UserId}", typeof(T).Name, entity.Id, _currentUser.UserId);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var existing = await BaseQuery().FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);
            if (existing == null)
            {
                _logger.LogWarning("❌ UpdateAsync failed: {EntityType} with Id = {Id} not found", typeof(T).Name, entity.Id);
                throw new KeyNotFoundException($"Entity with Id {entity.Id} not found.");
            }

            _context.Entry(existing).CurrentValues.SetValues(entity);
            _logger.LogInformation("✏️ UpdateAsync: {EntityType} Id = {Id} updated by User {UserId}", typeof(T).Name, entity.Id, _currentUser.UserId);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            _logger.LogInformation("🗑️ SoftDelete: {EntityType} Id = {Id} by User {UserId}", typeof(T).Name, entity.Id, _currentUser.UserId);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var query = BaseQuery();
            if (predicate != null)
                query = query.Where(predicate);

            return await query.CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await BaseQuery().AnyAsync(predicate, cancellationToken);

        public IQueryable<T> AsQueryable() => BaseQuery();

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var affected = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("💾 SaveChangesAsync: {Count} changes saved for {EntityType}", affected, typeof(T).Name);
            return affected > 0;
        }

        public async Task<BaseResponse<PaginatedResult<T>>> GetAllPaginatedAsync(
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<T, bool>>? filter = null,
            string? sortField = null,
            string? sortDirection = "asc",
            CancellationToken cancellationToken = default)
        {
            var query = BaseQuery();

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var sortExpr = $"{sortField} {sortDirection}";
                query = query.OrderBy(sortExpr);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            _logger.LogInformation("📄 GetAllPaginatedAsync: {EntityType}, Page {Page}, Size {Size}, Total {Total}",
                typeof(T).Name, pageIndex, pageSize, totalCount);

            var paginated = PaginatedResult<T>.Create(items, pageIndex, pageSize, totalCount);
            return BaseResponse<PaginatedResult<T>>.Success(paginated, "List retrieved", "00");
        }

        public async Task<BaseResponse<PaginatedResult<TDto>>> GetAllPaginatedAsync<TDto>(
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<T, bool>>? filter = null,
            string? sortField = null,
            string? sortDirection = "asc",
            CancellationToken cancellationToken = default)
        {
            var query = BaseQuery();

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var sortExpr = $"{sortField} {sortDirection}";
                query = query.OrderBy(sortExpr);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize)
                                   .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                                   .ToListAsync(cancellationToken);

            _logger.LogInformation("📄 GetAllPaginatedAsync<{Dto}>: {EntityType}, Page {Page}, Size {Size}, Total {Total}",
                typeof(TDto).Name, typeof(T).Name, pageIndex, pageSize, totalCount);

            var paginated = PaginatedResult<TDto>.Create(items, pageIndex, pageSize, totalCount);
            return BaseResponse<PaginatedResult<TDto>>.Success(paginated, "List retrieved", "00");
        }

        public async Task<T?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            var prop = typeof(T).GetProperty("Code");
            if (prop == null)
                throw new InvalidOperationException($"'{typeof(T).Name}' does not contain 'Code' property.");

            return await BaseQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => EF.Property<string>(x, "Code") == code, cancellationToken);
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null)
            => predicate != null ? BaseQuery().Where(predicate) : BaseQuery();

        public IQueryable<T> List(ISpecification<T> specification)
            => specification?.Apply(BaseQuery()) ?? BaseQuery();

        public async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
            => await List(specification).FirstOrDefaultAsync(cancellationToken);

        public async Task<bool> AnyByTenantAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await BaseQuery().AnyAsync(predicate, cancellationToken);

        public async Task<List<T>> GetAllByTenantAsync(CancellationToken cancellationToken = default)
            => await BaseQuery().ToListAsync(cancellationToken);
    }

}


