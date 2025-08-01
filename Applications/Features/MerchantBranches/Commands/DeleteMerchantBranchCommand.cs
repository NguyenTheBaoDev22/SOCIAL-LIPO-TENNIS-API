using MediatR;

namespace Applications.Features.MerchantBranches.Commands
{
    public class DeleteMerchantBranchCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
