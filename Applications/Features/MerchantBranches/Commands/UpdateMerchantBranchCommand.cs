using Applications.Features.MerchantBranches.Dtos;
using MediatR;

namespace Applications.Features.MerchantBranches.Commands
{
    public class UpdateMerchantBranchCommand : IRequest<MerchantBranchDto>
    {
        public Guid Id { get; set; }
        public string MerchantBranchCode { get; set; }
        public string BranchName { get; set; }
        public string MerchantCode { get; set; }
        public Guid MerchantId { get; set; }
        public string BranchEmail { get; set; }
        public int VerificationAttempts { get; set; } = 0;
        public string VerificationStatus { get; set; } = "Pending";
        public List<string> ExteriorImages { get; set; }
        public List<string> InteriorImages { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string CommuneCode { get; set; }
        public string CommuneName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; } = "Inactive";
    }
}
