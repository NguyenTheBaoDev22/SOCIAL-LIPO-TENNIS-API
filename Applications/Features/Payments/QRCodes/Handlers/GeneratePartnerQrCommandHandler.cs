using Applications.Features.Payments.QRCodes.Commands;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities.Partners;
using Core.Enumerables;
using MediatR;
using Serilog;
using Shared.Interfaces;
using Shared.Results;

namespace Applications.Features.Payments.QRCodes.Handlers;

/// <summary>
/// Handler xử lý yêu cầu tạo mã QR động từ Partner gửi lên.
/// </summary>
public class GeneratePartnerQrCommandHandler : IRequestHandler<GeneratePartnerQrCommand, BaseResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IPaymentGatewayService _paymentGatewayService;

    public GeneratePartnerQrCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IPaymentGatewayService paymentGatewayService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _paymentGatewayService = paymentGatewayService;
    }

    public async Task<BaseResponse<string>> Handle(GeneratePartnerQrCommand request, CancellationToken cancellationToken)
    {
        var traceId = _currentUser.TraceId ?? Guid.NewGuid().ToString("N");

        Log.Information("[{TraceId}] ▶️ Nhận yêu cầu tạo QR từ partner: {@Request}", traceId, request);

        //// === [B1] Xác thực merchant, branch, terminal ===

        //var merchant = await _unitOfWork.MerchantRepositories.FindByCodeAsync(request.MerchantCode);
        //if (merchant is null)
        //{
        //    Log.Warning("[{TraceId}] ❌ Merchant không tồn tại: {Code}", traceId, request.MerchantCode);
        //    return BaseResponse<string>.Error("Merchant not found", ErrorCodes.Merchant_NotFound);
        //}

        //var branch = await _unitOfWork.MerchantBranchRepositories.FindByCodeAsync(merchant.MerchantCode);
        //if (branch is null)
        //{
        //    Log.Warning("[{TraceId}] ❌ MerchantBranch không tồn tại: {Code}", traceId, request.MerchantBranchCode);
        //    return BaseResponse<string>.Error("Merchant branch not found", ErrorCodes.MerchantBranch_NotFound);
        //}

        //var terminal = await _unitOfWork.PaymentTerminalRepositories.FindByCodeAsync(branch.Id, request.PaymentTerminal);
        //if (terminal is null)
        //{
        //    Log.Warning("[{TraceId}] ❌ Terminal không tồn tại: {Code}", traceId, request.PaymentTerminal);
        //    return BaseResponse<string>.Error("Terminal not found", ErrorCodes.PaymentTerminal_NotFound);
        //}

        // === [B2] Tạo PartnerOrder để lưu lịch sử yêu cầu từ Partner ===

        var partnerOrder = new PartnerOrder
        {
            //PartnerId = _currentUser.PartnerId ?? Guid.Empty,
            //PartnerCode = _currentUser.PartnerCode ?? "Unknown",

            MerchantCode = request.MerchantCode,
            MerchantBranchCode = request.MerchantBranchCode,
            PaymentTerminalCode = request.PaymentTerminal,
            QrType = request.QrType,
            Amount = request.Amount,
            PurposeOfTransaction = request.PurposeOfTransaction,
            OrderCode = request.OrderCode,
            Ipn = request.Ipn,

            Status = PartnerOrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            RequestRawJson = System.Text.Json.JsonSerializer.Serialize(request),
            SourceIpAddress = _currentUser.IpAddress,
            UserAgent = _currentUser.UserAgent,
            TraceId = traceId,
        };

        await _unitOfWork.PartnerOrderRepositories.AddAsync(partnerOrder, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Log.Information("[{TraceId}] 📝 Đã tạo PartnerOrder với ID: {PartnerOrderId}", "#traceId#", partnerOrder.Id);//traceId

        // === [B3] Gọi PG để tạo QR ===

        try
        {
            var qrResult = await _paymentGatewayService.GenerateQrAsync(new PaymentGatewayQrRequest
            {
                MerchantCode = request.MerchantCode,
                MerchantBranchCode = request.MerchantBranchCode,
                PaymentTerminal = request.PaymentTerminal,
                Amount = request.Amount,
                QrType = request.QrType,
                PurposeOfTransaction = request.PurposeOfTransaction,
                OrderCode = request.OrderCode,
                Ipn = request.Ipn
            }, cancellationToken);

            partnerOrder.QrContent = qrResult;// qrResult.QrContent;
            partnerOrder.QrExpiredAt = null;// qrResult.ExpiredAt;
            partnerOrder.Status = PartnerOrderStatus.QrGenerated;
            partnerOrder.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.PartnerOrderRepositories.UpdateAsync(partnerOrder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            Log.Information("[{TraceId}] ✅ Đã tạo QR thành công: {@QrResult}", traceId, qrResult);

            return BaseResponse<string>.Success(qrResult, "QR generated successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "[{TraceId}] ❌ Lỗi khi gọi PG để tạo QR", traceId);

            partnerOrder.Status = PartnerOrderStatus.Failed;
            partnerOrder.UpdatedAt = DateTime.UtcNow;
            partnerOrder.PurposeOfTransaction += $" [ERROR: {ex.Message}]";
            await _unitOfWork.PartnerOrderRepositories.UpdateAsync(partnerOrder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<string>.Error("QR generation failed", ErrorCodes.Payment_GenerateQrFailed);
        }
    }
}
