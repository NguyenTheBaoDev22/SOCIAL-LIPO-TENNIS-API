using Core.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface IMerchantBranchRepository : IBaseRepository<MerchantBranch>
    {
        Task<MerchantBranch> GetWithMerchantByIdAsync(Guid merchantBranchId);
        Task<bool> ExistsByBranchTaxNumberAsync(string branchTaxNumber);
        /// <summary>
        /// Tìm MerchantBranch theo mã chi nhánh và MerchantId (đảm bảo duy nhất trong Merchant).
        /// </summary>
        Task<MerchantBranch?> FindByCodeAsync(Guid merchantId, string branchCode, CancellationToken cancellationToken = default);
    }
}
