using Applications.Features.Merchants.Commands;
using Applications.Features.Merchants.DTOs;
using Applications.Interfaces.Repositories;
using Core.Entities;
using Core.Enumerables;
using MediatR;
using Shared.Results;

namespace Applications.Features.Merchants.Handlers
{
    public class AddPaymentTerminalCommandHandler : IRequestHandler<AddPaymentTerminalCommand, BaseResponse<AddPaymentTerminalRes>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPaymentTerminalCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<AddPaymentTerminalRes>> Handle(AddPaymentTerminalCommand request, CancellationToken cancellationToken)
        {
            // Load branch và merchant
            var branch = await _unitOfWork.MerchantBranchRepositories.GetWithMerchantByIdAsync(request.MerchantBranchId);
            if (branch == null)
                return BaseResponse<AddPaymentTerminalRes>.Error("MerchantBranch not found", ErrorCodes.Merchant_InvalidBranch);

            var merchant = branch.Merchant;

            // ✅ Kiểm tra tồn tại Merchant (dù branch đã có Merchant, nhưng vẫn xác minh khớp)
            if (merchant == null || merchant.Id != request.MerchantId)
                return BaseResponse<AddPaymentTerminalRes>.Error("Merchant not found or does not match MerchantBranch", ErrorCodes.Merchant_NotFound);

            // ✅ Kiểm tra MerchantCode khớp với MerchantId
            if (!string.Equals(merchant.MerchantCode, request.MerchantCode, StringComparison.OrdinalIgnoreCase))
                return BaseResponse<AddPaymentTerminalRes>.Error("MerchantCode does not match MerchantId", ErrorCodes.Merchant_CodeMismatch);

            // ✅ Kiểm tra MerchantBranchCode khớp với Id
            if (!string.Equals(branch.MerchantBranchCode, request.MerchantBranchCode, StringComparison.OrdinalIgnoreCase))
                return BaseResponse<AddPaymentTerminalRes>.Error("MerchantBranchCode does not match MerchantBranchId", ErrorCodes.MerchantBranch_CodeMismatch);

            // ✅ Kiểm tra Merchant chưa active
            if (!merchant.IsActive)
                return BaseResponse<AddPaymentTerminalRes>.Error("Merchant is not active", ErrorCodes.Merchant_Inactive);

            // ✅ Kiểm tra Branch chưa active
            if (branch.Status != EBranchStatus.Active)
                return BaseResponse<AddPaymentTerminalRes>.Error("MerchantBranch is not active", ErrorCodes.MerchantBranch_Inactive);

            // ✅ Kiểm tra trùng SerialNumber nếu có
            if (!string.IsNullOrWhiteSpace(request.SerialNumber))
            {
                bool existsSerial = await _unitOfWork.PaymentTerminalRepositories
                    .ExistsActiveSerialNumberAsync(request.SerialNumber);

                if (existsSerial)
                {
                    return BaseResponse<AddPaymentTerminalRes>.Error(
                        "SerialNumber already exists.",
                        ErrorCodes.Terminal_DuplicateSerialNumber);
                }
            }

            // ✅ Sinh TerminalCode duy nhất trong phạm vi branch
            var usedCodes = branch.Terminals?.Select(t => t.TerminalCode).ToHashSet() ?? new HashSet<string>();
            string terminalCode = Enumerable.Range(1, 100)
                .Select(i => i.ToString("D2"))
                .FirstOrDefault(code => !usedCodes.Contains(code));

            if (terminalCode == null)
                return BaseResponse<AddPaymentTerminalRes>.Error("Maximum number of terminals reached", ErrorCodes.Terminal_ExceedLimit);

            // ✅ Tạo terminal mới
            var terminal = new PaymentTerminal
            {
                TerminalCode = terminalCode,
                TerminalName = request.TerminalName,
                DeviceType = request.DeviceType,
                SerialNumber = request.SerialNumber,
                MerchantBranchId = branch.Id,
                MerchantBranchCode = branch.MerchantBranchCode,
                MerchantCode = merchant.MerchantCode,
                CombinedIdentifier = $"{merchant.ZenPayMasterMerchantCode}{merchant.MerchantCode}{branch.MerchantBranchCode}{terminalCode}"
               // ActiveCallbackUrl = request.ActiveCallbackUrl ?? ""
            };

            await _unitOfWork.PaymentTerminalRepositories.AddAsync(terminal);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<AddPaymentTerminalRes>.Success(new AddPaymentTerminalRes
            {
                TerminalId = terminal.Id,
                TerminalCode = terminal.TerminalCode,
                CombinedIdentifier = terminal.CombinedIdentifier,

            });
        }


    }


}
