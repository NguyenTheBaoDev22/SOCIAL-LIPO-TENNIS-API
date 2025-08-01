using Core.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface ISmsLogRepository : IBaseRepository<SmsLog>
    {
        Task<List<SmsRetryQueue>> GetPendingRetriesAsync(int maxRetry, int take);
        Task AddRetryQueueAsync(SmsRetryQueue retry);
    }
}
