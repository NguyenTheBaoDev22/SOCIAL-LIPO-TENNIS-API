using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistences.Repositories
{
    public class SmsRetryQueueRepository : BaseRepository<SmsRetryQueue>, ISmsRetryQueueRepository
    {
        public SmsRetryQueueRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<SmsRetryQueue>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        public async Task<IEnumerable<SmsRetryQueue>> FindAsync(Expression<Func<SmsRetryQueue, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.SmsRetryQueues
                .Where(predicate)
                .ToListAsync(cancellationToken);
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
    }


}
