using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.AppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Infrastructure.Persistences.Repositories
{
    public class UserRoleAssignmentRepository : BaseRepository<UserRoleAssignment>, IUserRoleAssignmentRepository
    {
        public UserRoleAssignmentRepository(AppDbContext context, IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<UserRoleAssignment>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        public async Task<List<Guid>> GetRoleIdsForUserAsync(Guid userId, Guid merchantId, Guid? merchantBranchId)
        {
            return await _context.UserRoleAssignments
                .Where(x => x.UserId == userId &&
                            x.MerchantId == merchantId &&
                            x.MerchantBranchId == merchantBranchId &&
                            !x.IsDeleted)
                .Select(x => x.RoleId)
                .ToListAsync();
        }

        public async Task AssignRoleAsync(Guid userId, Guid roleId, Guid merchantId, Guid? merchantBranchId)
        {
            var assignment = new UserRoleAssignment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = roleId,
                MerchantId = merchantId,
                MerchantBranchId = merchantBranchId,
                CreatedAt = DateTime.UtcNow
            };

            await AddAsync(assignment);
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid roleId, Guid merchantId, Guid? merchantBranchId, CancellationToken cancellationToken = default)
        {
            return await _context.UserRoleAssignments
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.RoleId == roleId &&
                    x.MerchantId == merchantId &&
                    x.MerchantBranchId == merchantBranchId &&
                    !x.IsDeleted, cancellationToken);
        }
    }
}
