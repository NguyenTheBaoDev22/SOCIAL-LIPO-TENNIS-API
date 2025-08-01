using Applications.Features.MerchantBranches.Dtos;
using MediatR;
using Shared.Results;

namespace Applications.Features.MerchantBranches.Queries
{
    // Kế thừa từ IRequest<BaseResponse<MerchantBranchStatusRes>>
    public class GetMerchantBranchStatusQuery : IRequest<BaseResponse<MerchantBranchStatusRes>>
    {
        public Guid MerchantBranchId { get; set; }

        public GetMerchantBranchStatusQuery(Guid merchantBranchId)
        {
            MerchantBranchId = merchantBranchId;
        }
    }
}
