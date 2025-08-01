using Core.Entities.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories
{
    public interface IProductCategoryRepository : IBaseRepository<ProductCategory>
    {
        Task<ProductCategory> GetCategoryByCodeAsync(string code, CancellationToken cancellationToken = default);
    }
}
