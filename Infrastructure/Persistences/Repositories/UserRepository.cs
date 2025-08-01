using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.AppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistences.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<User>> baseLogger,
            ILogger<UserRepository> logger)
            : base(context, mapper, currentUser, baseLogger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Lấy người dùng theo username (không phân biệt hoa thường)
        /// </summary>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            _logger.LogInformation("🔍 Get user by username: {Username}", username);
            return await _dbSet
                .AsNoTracking()
                .Include(u => u.UserRoleAssignments)
                    .ThenInclude(ura => ura.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        /// <summary>
        /// Kiểm tra username đã tồn tại hay chưa
        /// </summary>
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            _logger.LogInformation("🔍 Check if username exists: {Username}", username);
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        // Implementing FindAsync method from IBaseRepository
        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken); // Matching return type with Task<IEnumerable<User>>
        }
        public async Task<User?> GetByIdWithRolesAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.UserRoleAssignments)
                    .ThenInclude(ra => ra.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }
        public async Task<User?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }

}
