using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistences.Repositories
{
    public class SmsLogRepository : BaseRepository<SmsLog>, ISmsLogRepository
    {
        private readonly AppDbContext _context;

        public SmsLogRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<SmsLog>> logger)
            : base(context, mapper, currentUser, logger)
        {
            _context = context;
        }

        // Implement FindAsync to match the IBaseRepository interface signature
        public async Task<IEnumerable<SmsLog>> FindAsync(Expression<Func<SmsLog, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);  // Ensures Task<IEnumerable<SmsLog>> return type
        }

        public async Task<List<SmsRetryQueue>> GetPendingRetriesAsync(int maxRetry, int take)
        {
            return await _context.SmsRetryQueues
                .Include(x => x.SmsLog)
                .Where(x => !x.IsDeleted && !x.IsDone && x.RetryCount < maxRetry)
                .OrderBy(x => x.LastTriedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task AddRetryQueueAsync(SmsRetryQueue retry)
        {
            await _context.SmsRetryQueues.AddAsync(retry);
        }
    }

}
