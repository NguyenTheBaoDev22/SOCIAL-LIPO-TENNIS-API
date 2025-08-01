using Applications.Features.MerchantBranches.Dtos;
using MediatR;

namespace Applications.Features.MerchantBranches.Queries
{
    public class GetMerchantBranchByIdQuery : IRequest<MerchantBranchDto>
    {
        public Guid Id { get; set; }
    }
}
