using Applications.Interfaces.Repositories.Partners;
using AutoMapper;
using Core.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistences.Repositories.Partners
{
    /// <summary>
    /// Repository xử lý PartnerOrder, kế thừa BaseRepository và cài đặt interface IPartnerOrderRepository.
    /// </summary>
    public class PartnerOrderRepository : BaseRepository<PartnerOrder>, IPartnerOrderRepository
    {
        private readonly AppDbContext _context;

        public PartnerOrderRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<PartnerOrder>> logger)
            : base(context, mapper, currentUser, logger)
        {
            _context = context;
        }

        /// <summary>
        /// Tìm đơn hàng Partner theo TransactionId để xử lý callback thanh toán.
        /// </summary>
        public async Task<PartnerOrder?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            //return await _context.PartnerOrders
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.TransactionId == transactionId && !x.IsDeleted, cancellationToken);
            return null;
        }
    }
}
