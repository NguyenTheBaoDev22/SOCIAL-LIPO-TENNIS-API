using Applications.Features.Merchants.DTOs;
using MediatR;

namespace Applications.Features.Merchants.Queries
{
    /// <summary>
    /// Query để lấy thông tin Merchant theo ID
    /// </summary>
    public class GetMerchantByIdQuery : IRequest<MerchantDto>
    {
        // ID của Merchant cần lấy thông tin
        public Guid Id { get; set; }
    }
}
