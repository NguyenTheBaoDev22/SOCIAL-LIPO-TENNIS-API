using Core.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface IMerchantRepository : IBaseRepository<Merchant>
    {
        Task<bool> ExistsByBusinessNoAsync(string businessRegistrationNo, CancellationToken cancellationToken = default);
        // Task<bool> ExistsByTaxNumberAsync(string taxNumber, CancellationToken cancellationToken = default);
        //Task<bool> ExistsByBankAccountAsync(string bankAccountNumber, CancellationToken cancellationToken = default);
        /// <summary>
        /// Tìm merchant theo mã code (duy nhất).
        /// </summary>
        Task<Merchant?> FindByCodeAsync(string merchantCode, CancellationToken cancellationToken = default);
    }
}
