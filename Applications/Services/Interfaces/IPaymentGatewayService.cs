using Applications.DTOs;
using Applications.Features.Payments.QRCodes.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    /// <summary>
    /// Interface cung cấp các hàm tích hợp với Payment Gateway (Napas, Payoo, v.v).
    /// </summary>
    public interface IPaymentGatewayService
    {
        /// <summary>
        /// Gọi PG để sinh mã QR động cho merchant.
        /// </summary>
        /// <param name="request">Thông tin yêu cầu tạo QR</param>
        /// <param name="cancellationToken">Token huỷ bỏ</param>
        /// <returns>Thông tin mã QR</returns>
        Task<string> GenerateQrAsync(PaymentGatewayQrRequest request, CancellationToken cancellationToken);
    }
}
