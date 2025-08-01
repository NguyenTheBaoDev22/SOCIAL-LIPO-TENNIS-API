using Core.Entities;
using Core.Entities.Partners;
using System.Threading;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    public interface IPartnerCallbackService
    {
        /// <summary>
        /// Gửi callback trạng thái merchant đến partner sau khi duyệt merchant.
        /// </summary>
        /// <param name="merchant">Thông tin merchant</param>
        /// <param name="branch">Chi nhánh được duyệt</param>
        /// <param name="cancellationToken">Token hủy tác vụ</param>
        /// <returns>Tuple (statusCode, responseContent, isSuccess)</returns>
        Task<(int StatusCode, string ResponseContent, bool IsSuccess)> SendMerchantStatusCallbackAsync(
            Merchant merchant,
            MerchantBranch branch,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gửi callback kết quả giao dịch đến partner.
        /// </summary>
        /// <param name="order">Đơn hàng partner</param>
        /// <param name="transaction">Giao dịch cần callback</param>
        /// <param name="cancellationToken">Token hủy tác vụ</param>
        /// <returns>True nếu callback thành công</returns>
        Task<bool> SendTransactionCallbackAsync(
            PartnerOrder order,
            PartnerTransactionCallbackLog transaction,
            CancellationToken cancellationToken = default);
    }
}
