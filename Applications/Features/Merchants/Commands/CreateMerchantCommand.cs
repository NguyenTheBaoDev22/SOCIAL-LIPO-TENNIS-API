using Applications.Features.Merchants.DTOs;
using MediatR;

namespace Applications.Features.Merchants.Commands
{
    /// <summary>
    /// Command để yêu cầu tạo mới Merchant
    /// </summary>
    public class CreateMerchantCommand : IRequest<MerchantDto>
    {
        // Tên của merchant
        public string MerchantName { get; set; }

        // Số đăng ký kinh doanh của merchant
        public string BusinessRegistrationNo { get; set; }

        // Địa chỉ của doanh nghiệp hoặc hộ kinh doanh
        public string BusinessAddress { get; set; }

        // Tên chủ sở hữu hoặc người đại diện
        public string OwnerName { get; set; }

        // Địa chỉ email của merchant
        public string Email { get; set; }

        // Số điện thoại của merchant
        public string Phone { get; set; }

        // Loại merchant: "Hộ kinh doanh" hoặc "Doanh nghiệp công ty"
        public string MerchantType { get; set; }

    }
}
