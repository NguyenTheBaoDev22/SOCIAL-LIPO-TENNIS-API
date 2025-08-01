using Applications.Features.MerchantBranches.Dtos;
using Applications.Features.MerchantBranches.Queries;
using Applications.Interfaces.Repositories;
using MediatR;
using Shared.Constants;
using Shared.Interfaces;
using Shared.Results;

namespace Applications.Features.MerchantBranches.Handlers;

public class GetMerchantBranchByTaxNumberHandler
    : IRequestHandler<GetMerchantBranchByTaxNumberQuery, BaseResponse<GetMerchantBranchByTaxNumberRes>>
{
    private readonly IMerchantBranchRepository _branchRepo;
    private readonly IMerchantRepository _merchantRepo;
    private readonly ICurrentUserService _currentUser;

    public GetMerchantBranchByTaxNumberHandler(
        IMerchantBranchRepository branchRepo,
        IMerchantRepository merchantRepo,
        ICurrentUserService currentUser)
    {
        _branchRepo = branchRepo;
        _merchantRepo = merchantRepo;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<GetMerchantBranchByTaxNumberRes>> Handle(
        GetMerchantBranchByTaxNumberQuery request, CancellationToken cancellationToken)
    {
        var traceId = _currentUser.TraceId;

        var branchs = await _branchRepo.FindAsync(x =>
            x.BranchTaxNumber == request.TaxNumber && !x.IsDeleted);

        if (branchs == null)
        {
            Console.WriteLine($"[{traceId}] ⚠️ Không tìm thấy MerchantBranch với MST: {request.TaxNumber}");
            return BaseResponse<GetMerchantBranchByTaxNumberRes>.Error(
                ErrorCodes.MerchantBranchTaxNo_NotFound,
                "Không tìm thấy chi nhánh với mã số thuế đã cung cấp.",
                traceId);
        }
        var branch = branchs.FirstOrDefault();
        var merchant = await _merchantRepo.GetByIdAsync(branch.MerchantId);
        if (merchant == null)
        {
            Console.WriteLine($"[{traceId}] ⚠️ Không tìm thấy Merchant với Id: {branch.MerchantId}");
            return BaseResponse<GetMerchantBranchByTaxNumberRes>.Error(
                ErrorCodes.Merchant_NotFound,
                "Không tìm thấy thương nhân tương ứng với chi nhánh.",
                traceId);
        }

        var res = new GetMerchantBranchByTaxNumberRes
        {
            MerchantId = merchant.Id,
            MerchantCode = merchant.MerchantCode,
            MerchantBranchId = branch.Id,
            MerchantBranchCode = branch.MerchantBranchCode,
            BranchName = branch.BranchName,
            BranchEmail = branch.BranchEmail
        };

        Console.WriteLine($"[{traceId}] ✅ Trả về thông tin MerchantBranch thành công.");
        return BaseResponse<GetMerchantBranchByTaxNumberRes>.Success(res, traceId);
    }
}
