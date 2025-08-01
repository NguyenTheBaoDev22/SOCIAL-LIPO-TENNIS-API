using Applications.Features.MerchantBranches.Dtos;
using MediatR;
using Shared.Results;

namespace Applications.Features.MerchantBranches.Commands
{
    public class ActivateMerchantBranchCommand : IRequest<BaseResponse<bool>>
    {
        public Guid MerchantBranchId { get; set; }
    }

}
