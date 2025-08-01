using Core.Entities.Partners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories.Partners
{
    /// <summary>
    /// Interface repository cho PartnerOrder, kế thừa từ IBaseRepository.
    /// </summary>
    public interface IPartnerOrderRepository : IBaseRepository<PartnerOrder>
    {
        /// <summary>
        /// Lấy PartnerOrder theo mã giao dịch TransactionId.
        /// </summary>
        Task<PartnerOrder?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);
    }
}
