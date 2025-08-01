using Applications.Features.MerchantBranches.Dtos;
using Applications.Features.MerchantBranches.Queries;
using Applications.Interfaces.Repositories;
using MediatR;

namespace Applications.Features.MerchantBranches.Handlers
{
    public class GetMerchantBranchByIdQueryHandler : IRequestHandler<GetMerchantBranchByIdQuery, MerchantBranchDto>
    {
        private readonly IMerchantBranchRepository _repository;

        public GetMerchantBranchByIdQueryHandler(IMerchantBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<MerchantBranchDto> Handle(GetMerchantBranchByIdQuery request, CancellationToken cancellationToken)
        {
            var merchantBranch = await _repository.GetByIdAsync(request.Id);
            return merchantBranch == null ? null : new MerchantBranchDto
            {
                Id = merchantBranch.Id,
                MerchantBranchCode = merchantBranch.MerchantBranchCode,
                BranchName = merchantBranch.BranchName,
                // Mapping các thuộc tính khác
            };
        }
    }
}
