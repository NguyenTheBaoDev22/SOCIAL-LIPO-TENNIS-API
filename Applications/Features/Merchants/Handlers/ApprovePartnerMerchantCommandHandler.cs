using Applications.Features.Merchants.Commands;
using Applications.Features.Merchants.Commands.Applications.Features.Merchants.Commands;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities;
using Core.Enumerables;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Interfaces;
using Shared.Results;

namespace Applications.Features.Merchants.Handlers;

/// <summary>
/// Handler xử lý phê duyệt merchant đến từ partner.
/// </summary>
public class ApprovePartnerMerchantCommandHandler : IRequestHandler<ApprovePartnerMerchantCommand, BaseResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPartnerCallbackService _partnerCallbackService;
    private readonly ICurrentUserService _currentUser;

    public ApprovePartnerMerchantCommandHandler(
        IUnitOfWork unitOfWork,
        IPartnerCallbackService partnerCallbackService,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _partnerCallbackService = partnerCallbackService;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<bool>> Handle(ApprovePartnerMerchantCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var traceId = _unitOfWork is ITraceable trace ? trace.TraceId : Guid.NewGuid().ToString();
            var approvedBy = _currentUser.UserId;

            Log.Information("[{TraceId}] 🔄 Bắt đầu xử lý phê duyệt merchant từ partner: {@Request}", traceId, request);

            var branch = await _unitOfWork.MerchantBranchRepositories.GetWithMerchantByIdAsync(request.MerchantBranchId);
            if (branch == null)
            {
                Log.Warning("[{TraceId}] ❌ Không tìm thấy branch với ID: {BranchId}", traceId, request.MerchantBranchId);
                return BaseResponse<bool>.Error("Branch not found", ErrorCodes.MerchantBranch_NotFound);
            }

            if (branch.Status == EBranchStatus.Active &&
                branch.VerificationStatus == VerificationStatusConstants.Approved)
            {
                Log.Information("[{TraceId}] ✅ Branch đã được duyệt trước đó. Bỏ qua xử lý.", traceId);
                return BaseResponse<bool>.Success(true, "Branch already approved");
            }

            // ✅ Cập nhật trạng thái branch
            branch.Status = EBranchStatus.Active;
            branch.VerificationStatus = VerificationStatusConstants.Approved;
            branch.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.MerchantBranchRepositories.UpdateAsync(branch, cancellationToken);

            // ✅ Cập nhật merchant
            branch.Merchant.IsActive = true;
            branch.Merchant.VerifiedAt = DateTime.UtcNow;
            branch.Merchant.VerifiedBy = approvedBy?.ToString();
            branch.Merchant.UpdatedAt = DateTime.UtcNow;
            branch.Merchant.UpdatedBy = approvedBy;
            await _unitOfWork.MerchantRepositories.UpdateAsync(branch.Merchant, cancellationToken);

            Log.Information("[{TraceId}] 📝 Đã cập nhật trạng thái branch và merchant thành công.", traceId);

            // ✅ Gửi callback nếu là merchant từ partner
            if (branch.Merchant.MerchantSource == MerchantSourceConstants.Partner &&
                !string.IsNullOrWhiteSpace(branch.ActiveCallbackUrl))
            {
                Log.Information("[{TraceId}] 📤 Đang gửi callback đến partner: {Url}", traceId, branch.ActiveCallbackUrl);

                var (statusCode, responseContent, isSuccess) =
                    await _partnerCallbackService.SendMerchantStatusCallbackAsync(branch.Merchant, branch, cancellationToken);

                Log.Information("[{TraceId}] 📥 Kết quả callback: StatusCode = {Code}, Success = {Success}",
                    traceId, statusCode, isSuccess);
            }

            Log.Information("[{TraceId}] ✅ Hoàn tất phê duyệt merchant và xử lý callback.", traceId);
            return BaseResponse<bool>.Success(true, "Merchant approved and callback handled.");
        }, cancellationToken);
    }
}
