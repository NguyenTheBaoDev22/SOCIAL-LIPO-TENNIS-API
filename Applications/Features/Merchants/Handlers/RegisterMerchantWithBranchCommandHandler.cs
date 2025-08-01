using Applications.Features.Merchants.DTOs;
using Applications.Interfaces;
using Applications.Interfaces.Repositories;
using Core.Entities;
using MediatR;
using Serilog;  // Sử dụng Serilog
using Shared.Interfaces;
using Shared.Results;

namespace Applications.Features.Merchants.Handlers
{
    public class RegisterMerchantWithBranchCommandHandler : IRequestHandler<RegisterMerchantWithBranchCommand, BaseResponse<RegisterMerchantWithBranchRes>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterMerchantWithBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<RegisterMerchantWithBranchRes>> Handle(RegisterMerchantWithBranchCommand request, CancellationToken cancellationToken)
        {
            var traceId = _unitOfWork is ITraceable traceable ? traceable.TraceId : Guid.NewGuid().ToString();

            try
            {
                Log.Information("[{TraceId}] ▶️ Start RegisterMerchantWithBranch | MerchantName: {MerchantName}", traceId, request.MerchantName);

                if (await _unitOfWork.MerchantRepositories.ExistsByBusinessNoAsync(request.BusinessRegistrationNo))
                {
                    Log.Warning("[{TraceId}] ⚠️ Duplicate BusinessRegistrationNo: {BusinessRegistrationNo}", traceId, request.BusinessRegistrationNo);
                    return BaseResponse<RegisterMerchantWithBranchRes>.Error(
                        $"Mã số thuế {request.BusinessRegistrationNo} đã được đăng ký trên hệ thống.",
                        ErrorCodes.MerchantBranch_DuplicateBranchTaxNo);
                }

                RegisterMerchantWithBranchRes result = new();

                await _unitOfWork.ExecuteWithTraceAsync(
                    async () =>
                    {
                        // Tạo TenantId nếu không có
                        var tenantId =  Guid.NewGuid(); // Sinh TenantId mới nếu không có
                        var merchant = new Merchant
                        {
                            MerchantName = request.MerchantName,
                            BusinessRegistrationNo = request.BusinessRegistrationNo,
                            BusinessAddress = request.BusinessAddress,
                            OwnerName = request.OwnerName,
                            PrimaryEmail = request.Email,
                            PrimaryPhone = request.Phone,
                            MerchantType = request.MerchantType,
                            OwnerIdCardFrontUrl = request.OwnerIdCardFrontUrl,
                            OwnerIdCardBackUrl = request.OwnerIdCardBackUrl,
                            OwnerIdCardNumber = request.OwnerIdCardNumber,
                            PrimaryTaxNumber = request.BusinessRegistrationNo,
                            BusinessRegistrationImageUrls = request.BusinessRegistrationImageUrls,
                            TenantId=tenantId
                        };

                        await _unitOfWork.MerchantRepositories.AddAsync(merchant);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);

                        var branch = new MerchantBranch
                        {
                            MerchantId = merchant.Id,
                            BranchName = request.MerchantName,
                            BranchAddress = request.BusinessAddress,
                            ProvinceName = request.ProvinceName,
                            CommuneName = request.CommuneName,
                            Latitude = 0,
                            Longitude = 0,
                            BranchEmail = request.Email,
                            BranchPhone = request.Phone,
                            ExteriorImages = request.ExteriorImagesUrl,
                            InteriorImages = request.InteriorImagesUrl,
                            CommuneCode = request.CommuneCode,
                            ProvinceCode = request.ProvinceCode,
                            MerchantCode = merchant.MerchantCode,
                            BankAccountHolder = request.BankAccountHolder,
                            BankAccountNumber = request.BankAccountNumber,
                            BankName = request.BankName,
                            SignedForm01AUrl = request.SignedForm01AUrl ?? "",
                            MerchantCategoryCode = request.MerchantCategoryCode,
                            BranchTaxNumber = request.BusinessRegistrationNo,
                            ActiveCallbackUrl = request.ActiveCallbackUrl ?? "",
                            TenantId = tenantId
                        };

                        await _unitOfWork.MerchantBranchRepositories.AddAsync(branch);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);

                        result = new RegisterMerchantWithBranchRes
                        {
                            MerchantId = merchant.Id,
                            MerchantCode = merchant.MerchantCode,
                            IsMerchantBranchActive = merchant.IsActive,
                            MerchantBranchCode = branch.MerchantBranchCode,
                            MerchantBranchId = branch.Id
                        };
                    },
                    traceId,
                    afterCommit: null, // hoặc async () => await _ipnService.Notify(...)
                    cancellationToken
                );

                Log.Information("[{TraceId}] ✅ Registered Merchant ({MerchantCode}) with Branch ({BranchName})", traceId, result.MerchantCode, request.MerchantName);

                return BaseResponse<RegisterMerchantWithBranchRes>.Success(result, "Merchant and Branch registered successfully", "00");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TraceId}] ❌ Unhandled exception while registering Merchant and Branch", traceId);
                return BaseResponse<RegisterMerchantWithBranchRes>.Error("Internal server error occurred.", "ERR_INTERNAL");
            }
        }




    }

}
