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
    public class ProductImportRepository : BaseRepository<ProductImport>, IProductImportRepository
    {
        private readonly AppDbContext _context;

        public ProductImportRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<ProductImport>> logger)
            : base(context, mapper, currentUser, logger)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductImport>> GetByProductIdAsync(Guid productId)
        {
            return await _context.ProductImports
                .Where(pi => pi.ProductId == productId && !pi.IsDeleted)
                .ToListAsync();
        }
    }
}
