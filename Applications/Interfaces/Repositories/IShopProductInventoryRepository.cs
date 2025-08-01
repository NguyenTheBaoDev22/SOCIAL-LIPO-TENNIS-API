using Core.Entities.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories
{
    //public interface IShopProductInventoryRepository : IBaseRepository<ShopProductInventory>
    //{
    //    Task<IEnumerable<ShopProductInventory>> GetInventoryByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    //}


    public interface IShopProductInventoryRepository : IBaseRepository<ShopProductInventory>
    {
        Task<List<ShopProductInventory>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<List<ShopProductInventory>> GetLatestInventoriesByBranchAsync(Guid merchantBranchId, int count = 10, CancellationToken cancellationToken = default);
    }
}
