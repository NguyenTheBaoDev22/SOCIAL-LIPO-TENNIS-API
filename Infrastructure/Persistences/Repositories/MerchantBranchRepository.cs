using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Infrastructure.Persistences.Repositories
{

    public class MerchantBranchRepository : BaseRepository<MerchantBranch>, IMerchantBranchRepository
    {
        public MerchantBranchRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<MerchantBranch>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        // Phương thức mới để lấy MerchantBranch và Merchant cùng lúc (eager loading)
        public async Task<MerchantBranch> GetWithMerchantByIdAsync(Guid merchantBranchId)
        {
            // Eager load Merchant khi truy vấn MerchantBranch
            return await _context.MerchantBranches
                .Include(b => b.Merchant)  // Eager load Merchant
                 .Include(b => b.Terminals) // ✅ Thêm dòng này để load danh sách thiết bị
                .FirstOrDefaultAsync(b => b.Id == merchantBranchId);
        }

        public async Task<bool> ExistsByBranchTaxNumberAsync(string branchTaxNumber)
        {
            if (string.IsNullOrWhiteSpace(branchTaxNumber))
                return false;

            return await _context.MerchantBranches
                .AnyAsync(b => b.BranchTaxNumber == branchTaxNumber);
        }
        public async Task<MerchantBranch?> FindByCodeAsync(Guid merchantId, string branchCode, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(branchCode))


                return null;

            return await _context.MerchantBranches
                .AsNoTracking()
                .Where(b => !b.IsDeleted && b.MerchantId == merchantId && b.MerchantBranchCode == branchCode)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
