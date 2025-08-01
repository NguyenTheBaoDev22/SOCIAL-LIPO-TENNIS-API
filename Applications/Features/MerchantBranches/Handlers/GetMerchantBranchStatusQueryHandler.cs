using Applications.Features.MerchantBranches.Dtos;
using Applications.Features.MerchantBranches.Queries;
using Applications.Interfaces.Repositories;
using MediatR;
using Shared.Results;

namespace Applications.Features.MerchantBranches.Handlers
{
    public class GetMerchantBranchStatusQueryHandler : IRequestHandler<GetMerchantBranchStatusQuery, BaseResponse<MerchantBranchStatusRes>>
    {

        private readonly IMerchantBranchRepository _merchantBranchRepository;

        public GetMerchantBranchStatusQueryHandler(IMerchantBranchRepository merchantBranchRepository)
        {
            _merchantBranchRepository = merchantBranchRepository;
        }

        public async Task<BaseResponse<MerchantBranchStatusRes>> Handle(GetMerchantBranchStatusQuery request, CancellationToken cancellationToken)
        {
            // Lấy thông tin MerchantBranch từ repository (đã eager load Merchant)
            var branch = await _merchantBranchRepository.GetWithMerchantByIdAsync(request.MerchantBranchId);

            if (branch == null)
            {
                return BaseResponse<MerchantBranchStatusRes>.Error("MerchantBranch not found", "404");
            }

            // Tạo MerchantBranchStatusRes và ánh xạ thông tin
            var result = new MerchantBranchStatusRes
            {
                MerchantId = branch.Merchant.Id,
                MerchantName = branch.Merchant.MerchantName,
                MerchantType = branch.Merchant.MerchantType,
                IsMerchantActive = branch.Merchant.IsActive,
                MerchantBranchId = branch.Id,
                BranchName = branch.BranchName,
                VerificationAttempts = branch.VerificationAttempts,
                VerificationStatus = branch.VerificationStatus,
                Status = branch.Status,
                MerchantBranchCode = branch.MerchantBranchCode,
                MerchantCode = branch.Merchant.MerchantCode,
            };

            // Trả về kết quả thành công
            return BaseResponse<MerchantBranchStatusRes>.Success(result, "Merchant Branch status retrieved successfully", "00");
        }

    }
}
