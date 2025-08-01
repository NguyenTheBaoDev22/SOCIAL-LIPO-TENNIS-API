using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.AppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Authorization;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistences.Repositories
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<RolePermission>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        /// <summary>
        /// Tìm RolePermission theo điều kiện
        /// </summary>
        public async Task<IEnumerable<RolePermission>> FindAsync(Expression<Func<RolePermission, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Lấy danh sách PermissionCode (string) theo danh sách RoleId
        /// </summary>
        public async Task<List<string>> GetPermissionCodesByRoleIdsAsync(IEnumerable<Guid> roleIds)
        {
            _logger.LogInformation("🔍 Getting permission codes for RoleIds: {RoleIds}", string.Join(", ", roleIds));
            return await _dbSet
                .Include(rp => rp.Permission)
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.Permission.Code.ToString())
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách PermissionCode theo RoleId
        /// </summary>
        public async Task<List<PermissionCode>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            _logger.LogInformation("🔍 Getting PermissionCode list for RoleId: {RoleId}", roleId);
            return await _dbSet
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission.Code)
                .Distinct()
                .ToListAsync();
        }
    }


}
