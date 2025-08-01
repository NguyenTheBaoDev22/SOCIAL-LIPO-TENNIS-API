using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.AppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Infrastructure.Persistences.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        private readonly ILogger<PermissionRepository> _logger;

        public PermissionRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<PermissionRepository> logger,
            ILogger<BaseRepository<Permission>> baseLogger)
            : base(context, mapper, currentUser, baseLogger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Lấy permission theo mã code (không phân biệt hoa thường)
        /// </summary>
        public async Task<Permission?> GetByCodeAsync(string code)
        {
            _logger.LogInformation("🔍 Get permission by code: {Code}", code);
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Code.ToString().ToLower() == code.ToLower());
        }

        /// <summary>
        /// Lấy toàn bộ danh sách quyền
        /// </summary>
        public async Task<List<Permission>> GetAllAsync()
        {
            _logger.LogInformation("📋 Get all permissions");
            return await _dbSet
                .AsNoTracking()
                .OrderBy(p => p.Code)
                .ToListAsync();
        }

        /// <summary>
        /// Kiểm tra quyền tồn tại theo code
        /// </summary>
        public async Task<bool> ExistsByCodeAsync(string code)
        {
            _logger.LogInformation("🔍 Check if permission exists: {Code}", code);
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(p => p.Code.ToString().ToLower() == code.ToLower());
        }
    }
}
