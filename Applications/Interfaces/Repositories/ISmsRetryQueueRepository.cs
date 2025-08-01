using Core.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface ISmsRetryQueueRepository : IBaseRepository<SmsRetryQueue>
    {
        Task<List<SmsRetryQueue>> GetPendingRetriesAsync(int maxRetry, int take);
    }
}
