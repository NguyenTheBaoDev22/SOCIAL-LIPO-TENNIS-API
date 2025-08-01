using Applications.Features.Merchants.DTOs;
using MediatR;

namespace Applications.Features.Merchants.Commands
{
    // <summary>
    /// Command để yêu cầu cập nhật Merchant
    /// </summary>
    public class UpdateMerchantCommand : IRequest<MerchantDto>
    {
        // ID của Merchant cần cập nhật
        public Guid Id { get; set; }

        public string MerchantName { get; set; }
        public string BusinessRegistrationNo { get; set; }
        public string BusinessAddress { get; set; }
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MerchantType { get; set; }
        public bool IsActive { get; set; }
    }
}
