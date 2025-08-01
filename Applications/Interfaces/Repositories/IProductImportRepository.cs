using Core.Entities.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories
{
    public interface IProductImportRepository : IBaseRepository<ProductImport>
    {
        /// <summary>
        /// Lấy tất cả các bản ghi ProductImport theo ProductId
        /// </summary>
        Task<IEnumerable<ProductImport>> GetByProductIdAsync(Guid productId);

        // Bạn có thể bổ sung thêm các phương thức đặc biệt khác nếu cần
    }
}
