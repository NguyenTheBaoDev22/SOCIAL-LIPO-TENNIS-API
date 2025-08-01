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
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<Product>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        public async Task<Product> GetProductByCodeAsync(string productCode, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.ProductCode == productCode && !p.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(p => p.ProductCategoryId == categoryId && !p.IsDeleted)
                .ToListAsync(cancellationToken);
        }
    }
}
