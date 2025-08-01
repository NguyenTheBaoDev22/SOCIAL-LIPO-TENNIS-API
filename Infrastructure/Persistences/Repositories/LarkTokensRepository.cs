using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistences.Repositories
{
    public class LarkTokensRepository : BaseRepository<LarkTokens>, ILarkTokensRepository
    {
        public LarkTokensRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<LarkTokensRepository> logger
        ) : base(context, mapper, currentUser, logger)
        {
        }

        // Custom method: lấy token còn hiệu lực của user
        public async Task<LarkTokens?> GetActiveTokenByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.CreatedBy == userId && x.Status == "Active" && x.AccessTokenExpiresAt > DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<LarkTokens?> GetLatestValidAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
