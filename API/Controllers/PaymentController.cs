using Applications.Features.Payments.QRCodes.Commands;
using Core.Enumerables;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Results;
using Shared.Results.Extensions;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Partner yêu cầu sinh mã QR động.
        /// </summary>
        /// <param name="command">Thông tin cần thiết để tạo QR</param>
        /// <returns>Chuỗi QR trả về từ hệ thống PG</returns>
        [HttpPost("partner/generate-dynamicQr")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateQr([FromBody] GeneratePartnerQrCommand command, CancellationToken cancellationToken)
        {
            var traceId = HttpContext.TraceIdentifier;
            Log.Information("[{TraceId}] ▶️ API /partner/qr/generate được gọi với payload: {@Command}", traceId, command);

            var response = await _mediator.Send(command, cancellationToken);

            Log.Information("[{TraceId}] ⏹️ API /partner/qr/generate trả về kết quả: {@Response}", traceId, response);

            return StatusCode(ErrorCodeMapper.ToHttpStatusCode(response.Code), response);
        }


        [HttpPost("dynamic-qr")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateDynamicQr([FromBody] GenerateQrCommand command, CancellationToken cancellationToken)
        {
            var traceId = HttpContext.TraceIdentifier;
            Log.Information("[{TraceId}] ▶️ API /partner/qr/generate được gọi với payload: {@Command}", traceId, command);

            // Mock kết quả API
            var result = new
            {
                orderId = Guid.NewGuid().ToString(),  // Mã đơn hàng ngẫu nhiên
                qrCode = GenerateRandomQrCode(),     // QR code ngẫu nhiên
                expirationInMinutes = 30,            // Thời gian hết hạn là 30 phút
                bankAccountNumber = "1234567890",    // Số tài khoản ngân hàng
                bankName = "Ngân hàng TMCP Á Châu",  // Tên ngân hàng
                bankAccountHolder = "Nguyễn Văn A"  // Chủ tài khoản
            };

            Log.Information("[{TraceId}] ⏹️ API /partner/qr/generate trả về kết quả: {@Response}", traceId, result);

            // Trả về phản hồi thành công với dữ liệu
            return Ok(BaseResponse<object>.Success(result));
        }
        // Sinh mã QR ngẫu nhiên
        private string GenerateRandomQrCode()
        {
            return Guid.NewGuid().ToString();  // Trả về chuỗi ngẫu nhiên làm mã QR
        }


        // Lớp command nhận đầu vào cho API sinh QR
        public class GenerateQrCommand
        {
            public Guid MerchantBranchId { get; set; }
            public Guid OrderId { get; set; }
            public decimal Amount { get; set; }
        }

        [HttpGet("merchant-branches/{merchantBranchId}/payment-methods")]
        public IActionResult GetPaymentMethods( Guid merchantBranchId)
        {
            // Nếu cần logic theo chi nhánh thì xử lý ở đây
            // Ví dụ: chỉ chi nhánh nào đó mới hỗ trợ CARD

            var result = PaymentMethodConst.DisplayNames.Select(x => new
            {
                Code = x.Key,
                Name = x.Value
            });


            return Ok(BaseResponse<object>.Success(result));
        }

    }
}
