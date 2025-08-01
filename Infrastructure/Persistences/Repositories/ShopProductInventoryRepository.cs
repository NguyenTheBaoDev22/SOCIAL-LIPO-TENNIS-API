using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.Shops;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistences.Repositories
{
    //public class ShopProductInventoryRepository : BaseRepository<ShopProductInventory>, IShopProductInventoryRepository
    //{
    //    public ShopProductInventoryRepository(AppDbContext context, 
    //        IMapper mapper,
    //        ICurrentUserService currentUser) : base(context, mapper,currentUser)
    //    {
    //    }

    //    // Implement FindAsync to match the IBaseRepository interface signature
    //    public async Task<IEnumerable<ShopProductInventory>> FindAsync(Expression<Func<ShopProductInventory, bool>> predicate, CancellationToken cancellationToken = default)
    //    {
    //        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);  // Ensures Task<IEnumerable<ShopProductInventory>> return type
    //    }

    //    public async Task<IEnumerable<ShopProductInventory>> GetInventoryByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    //    {
    //        return await Query()
    //            .AsNoTracking()
    //            .Where(i => i.ProductId == productId)
    //            .ToListAsync(cancellationToken);
    //    }
    //}
    /// <summary>
    /// Repository xử lý logic riêng cho bảng ShopProductInventory.
    /// </summary>
    public class ShopProductInventoryRepository : BaseRepository<ShopProductInventory>, IShopProductInventoryRepository
    {
        public ShopProductInventoryRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<ShopProductInventory>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        /// <summary>
        /// Lấy toàn bộ phiếu kiểm kê theo ProductId.
        /// </summary>
        public async Task<List<ShopProductInventory>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting inventory records for ProductId: {ProductId}", productId);
            return await _dbSet
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.AuditDate)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Lấy các phiếu kiểm kê gần nhất theo chi nhánh.
        /// </summary>
        public async Task<List<ShopProductInventory>> GetLatestInventoriesByBranchAsync(Guid merchantBranchId, int count = 10, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting latest {Count} inventory records for BranchId: {BranchId}", count, merchantBranchId);
            return await _dbSet
                .Where(x => x.MerchantBranchId == merchantBranchId)
                .OrderByDescending(x => x.AuditDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }

}