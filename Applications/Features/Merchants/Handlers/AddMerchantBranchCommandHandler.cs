using Applications.Features.Merchants.DTOs;
using Applications.Interfaces.Repositories;
using Core.Entities;
using MediatR;
using Serilog;
using Shared.Results;

namespace Applications.Features.Merchants.Handlers
{
    public class AddMerchantBranchCommandHandler : IRequestHandler<AddMerchantBranchCommand, BaseResponse<AddMerchantBranchRes>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddMerchantBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<AddMerchantBranchRes>> Handle(AddMerchantBranchCommand request, CancellationToken cancellationToken)
        {
            Log.Information("➡️ Adding new branch | MerchantId: {MerchantId}, MerchantCode: {MerchantCode}", request.MerchantId, request.MerchantCode);

            var merchant = await _unitOfWork.MerchantRepositories.GetByIdAsync(request.MerchantId);
            if (merchant == null)
            {
                Log.Warning("❌ Merchant not found | MerchantId: {MerchantId}", request.MerchantId);
                return BaseResponse<AddMerchantBranchRes>.Error("Merchant not found.", ErrorCodes.Merchant_NotFound);
            }

            if (!string.Equals(merchant.MerchantCode, request.MerchantCode, StringComparison.OrdinalIgnoreCase))
            {
                Log.Warning("❌ MerchantCode mismatch | Input: {InputCode}, DB: {DbCode}", request.MerchantCode, merchant.MerchantCode);
                return BaseResponse<AddMerchantBranchRes>.Error("Invalid MerchantCode.", ErrorCodes.Merchant_InvalidCode);
            }

            if (!string.IsNullOrWhiteSpace(request.BranchTaxNumber))
            {
                var existed = await _unitOfWork.MerchantBranchRepositories.ExistsByBranchTaxNumberAsync(request.BranchTaxNumber);
                if (existed)
                {
                    Log.Warning("❌ Duplicate BranchTaxNumber: {BranchTaxNumber}", request.BranchTaxNumber);
                    return BaseResponse<AddMerchantBranchRes>.Error("Branch tax number already exists.", ErrorCodes.MerchantBranch_DuplicateBranchTaxNo);
                }
            }

            // ✅ Gói toàn bộ xử lý trong transaction-safe strategy
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var newBranch = new MerchantBranch
                {
                    MerchantId = merchant.Id,
                    MerchantCode = merchant.MerchantCode,
                    BranchName = request.BranchName,
                    BranchAddress = request.BranchAddress,
                    ProvinceCode = request.ProvinceCode,
                    ProvinceName = request.ProvinceName,
                    CommuneCode = request.CommuneCode,
                    CommuneName = request.CommuneName,
                    BranchEmail = request.BranchEmail,
                    BranchPhone = request.BranchPhone,
                    ExteriorImages = request.ExteriorImages,
                    InteriorImages = request.InteriorImages,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    BankAccountNumber = request.BankAccountNumber,
                    BankName = request.BankName,
                    BankAccountHolder = request.BankAccountHolder,
                    SignedForm01AUrl = request.SignedForm01AUrl,
                    MerchantCategoryCode = request.MerchantCategoryCode,
                    BranchTaxNumber = request.BranchTaxNumber,
                    IsHeadOffice = false,
                    ActiveCallbackUrl = request.ActiveCallbackUrl ?? ""
                };

                await _unitOfWork.MerchantBranchRepositories.AddAsync(newBranch);
                // ✅ SaveChanges sẽ được gọi trong transaction logic

                Log.Information("✅ Successfully added branch | MerchantCode: {MerchantCode}, BranchCode: {BranchCode}", merchant.MerchantCode, newBranch.MerchantBranchCode);

                var result = new AddMerchantBranchRes
                {
                    MerchantId = merchant.Id,
                    MerchantCode = merchant.MerchantCode,
                    MerchantBranchId = newBranch.Id,
                    MerchantBranchCode = newBranch.MerchantBranchCode,
                    IsMerchantBranchActive = newBranch.Status == Core.Enumerables.EBranchStatus.Active
                };

                return BaseResponse<AddMerchantBranchRes>.Success(result, "Branch added successfully", "00");
            }, cancellationToken);
        }

    }
}
