using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.Shops;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistences.Repositories
{
    public class ProductCategoryRepository : BaseRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<ProductCategory>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        public async Task<ProductCategory> GetCategoryByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted, cancellationToken);
        }
    }
}
