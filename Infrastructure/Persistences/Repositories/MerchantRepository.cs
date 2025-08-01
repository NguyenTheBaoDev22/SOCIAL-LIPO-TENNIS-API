using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Infrastructure.Persistences.Repositories
{

    public class MerchantRepository : BaseRepository<Merchant>, IMerchantRepository
    {
        public MerchantRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<Merchant>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        public async Task<bool> ExistsByBusinessNoAsync(string businessRegistrationNo, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(m =>
                m.BusinessRegistrationNo == businessRegistrationNo && !m.IsDeleted, cancellationToken);
        }

        //public async Task<bool> ExistsByTaxNumberAsync(string taxNumber, CancellationToken cancellationToken = default)
        //{
        //    return await _dbSet.AnyAsync(m =>
        //        m.TaxNumber == taxNumber && !m.IsDeleted, cancellationToken);
        //}

        //public async Task<bool> ExistsByBankAccountAsync(string bankAccountNumber, CancellationToken cancellationToken = default)
        //{
        //    return await _dbSet.AnyAsync(m =>
        //        m.BankAccountNumber == bankAccountNumber && !m.IsDeleted, cancellationToken);
        //}
        /// <summary>
        /// Tìm merchant theo mã MerchantCode.
        /// </summary>
        public async Task<Merchant?> FindByCodeAsync(string merchantCode, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(m => m.MerchantCode == merchantCode && !m.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
