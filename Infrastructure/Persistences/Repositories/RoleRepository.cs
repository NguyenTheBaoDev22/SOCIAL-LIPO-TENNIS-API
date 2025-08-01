using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.AppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistences.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<Role>> baseLogger,
            ILogger<RoleRepository> logger)
            : base(context, mapper, currentUser, baseLogger)
        {
            _logger = logger;
        }

        // Implement FindAsync to match the IBaseRepository interface signature
        public async Task<IEnumerable<Role>> FindAsync(Expression<Func<Role, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);  // Ensures Task<IEnumerable<Role>> return type
        }

        public async Task<bool> ExistsAsync(Guid roleId)
        {
            _logger.LogDebug("🔍 Checking if role exists: {RoleId}", roleId);
            return await _context.Roles.AnyAsync(r => r.Id == roleId);
        }

        public async Task<Role?> GetByIdAsync(Guid roleId)
        {
            _logger.LogDebug("🔍 Getting role by ID: {RoleId}", roleId);
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        }
    }

}
