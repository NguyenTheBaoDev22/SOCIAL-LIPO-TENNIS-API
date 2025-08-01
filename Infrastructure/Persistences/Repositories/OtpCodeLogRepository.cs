using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.AppUsers;
using Core.Enumerables;
using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistences.Repositories
{
    public class OtpCodeLogRepository : BaseRepository<OtpCodeLog>, IOtpCodeLogRepository
    {
        public OtpCodeLogRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<OtpCodeLog>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        /// <summary>
        /// Lấy OTP gần nhất chưa hết hạn và chưa xác minh.
        /// </summary>
        public async Task<OtpCodeLog?> GetLatestActiveCodeAsync(string phoneNumber, string purpose, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.PhoneNumber == phoneNumber
                            && x.Purpose == purpose
                            && !x.IsVerified
                            && !x.IsDeleted
                            && x.ExpireAt >= DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Đánh dấu OTP là đã sử dụng.
        /// </summary>
        public async Task MarkAsUsedAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var record = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

            if (record != null)
            {
                record.IsVerified = true;
                _dbSet.Update(record);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Tìm tất cả OTP theo điều kiện.
        /// </summary>
        public async Task<List<OtpCodeLog>> FindAllAsync(Expression<Func<OtpCodeLog, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }
        public async Task<OtpCodeLog?> GetLatestUnverifiedOtpAsync(
    string phoneNumber,
    string otpCode,
    string purpose,
    CancellationToken cancellationToken = default)
        {
            return await _context.OtpCodeLogs
                .Where(x =>
                    x.PhoneNumber == phoneNumber &&
                    x.OtpCode == otpCode &&
                    x.Purpose == purpose &&
                    !x.IsVerified &&
                    x.ExpireAt > DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> CountOtpRequestsInPeriodAsync(string phoneNumber, string purpose, TimeSpan period, CancellationToken cancellationToken = default)
        {
            var since = DateTime.UtcNow.Subtract(period);

            return await _context.OtpCodeLogs
                .Where(x => x.PhoneNumber == phoneNumber &&
                            x.Purpose == purpose &&
                            x.CreatedAt >= since)
                .CountAsync(cancellationToken);
        }

    }
}
