using Applications.DTOs;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        /// <summary>
        /// Tạo đơn hàng thanh toán nhanh và trả về mã đơn hàng, mã thanh toán, và thời gian hết hạn.
        /// </summary>
        [HttpPost("quick-order")]
        public async Task<ActionResult<BaseResponse<object>>> CreateQuickOrder([FromBody] CreateQuickPaymentCommand command)
        {
            // Mock việc tạo đơn hàng
            var payment = new PaymentResponse
            {
                OrderId = Guid.NewGuid(),
                OrderCode = GenerateOrderCode(),  // Tạo OrderCode ngẫu nhiên
                TotalAmount = command.TotalAmount,
                MerchantBranchId = command.MerchantBranchId,
              
            };

            // Trả về OrderId, OrderCode và thời gian hết hạn
            var result = new
            {
                OrderId = payment.OrderId,
                OrderCode = payment.OrderCode,
                ExpirationInMinutes = payment.ExpirationInMinutes
            };

            return Ok(BaseResponse<object>.Success(result));
        }

        // Lớp command nhận đầu vào cho API tạo mã thanh toán
        public class CreateQuickPaymentCommand
        {
            public Guid MerchantBranchId { get; set; }
            public decimal TotalAmount { get; set; }  // Số tiền thanh toán
            public Guid? ProductId { get; set; }
            public string? ProductName { get; set; }
            public string? CustomerName { get; set; }
            public bool IsInvoiceRequired { get; set; }
            public string PaymentMethod { get; set; }

        }

        // Lớp phản hồi thông tin thanh toán
        public class PaymentResponse
        {
            public Guid OrderId { get; set; }
            public string OrderCode { get; set; }
            public decimal TotalAmount { get; set; }
            public Guid MerchantBranchId { get; set; }
            public int ExpirationInMinutes { get; set; }
        }



        /// <summary>
        /// Kiểm tra trạng thái thanh toán của đơn hàng
        /// </summary>
        [HttpGet("payment-status/{orderId}")]
        public async Task<ActionResult<BaseResponse<OrderPaymentStatusResponse>>> GetOrderPaymentStatus([FromRoute] Guid orderId)
        {
            // Mock trạng thái thanh toán đơn hàng (gắn cứng là "Pending")
            var response = new OrderPaymentStatusResponse
            {
                OrderPaymentStatus = "Pending"  // Trạng thái thanh toán giả định
            };

            // Trả về phản hồi với trạng thái thanh toán của đơn hàng
            return Ok(BaseResponse<OrderPaymentStatusResponse>.Success(response));
        }
        // Lớp phản hồi cho trạng thái thanh toán đơn hàng
        public class OrderPaymentStatusResponse
        {
            public string OrderPaymentStatus { get; set; }  // Trạng thái thanh toán của đơn hàng
        }




        /// <summary>
        /// Tạo đơn hàng và trả về thông tin đơn hàng (OrderId, OrderCode)
        /// </summary>
        [HttpPost("create-order")]
        public async Task<ActionResult<BaseResponse<object>>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            // Mock việc tạo đơn hàng
            var order = new OrderResponse
            {
                OrderId = Guid.NewGuid(),
                OrderCode = GenerateOrderCode(),  // Tạo OrderCode ngẫu nhiên
                MerchantBranchId = command.MerchantBranchId,
                OrderItems = command.OrderItems.Select(item => new OrderItemResponse
                {
                    ProductId = item.ProductId,
                    ProductSellingPrice = item.ProductSellingPrice,
                    Quantities = item.Quantities,
                    TotalPrice = item.TotalPrice
                }).ToList(),
                TotalOrderPrice = command.OrderItems.Sum(item => item.TotalPrice),
                IsInvoiceRequired = command.IsInvoiceRequired,
                CustomerInfo = command.IsInvoiceRequired ? new CustomerInfo
                {
                    Name = command.CustomerInfo.Name,
                    PhoneNumber = command.CustomerInfo.PhoneNumber,
                    CCCD = command.CustomerInfo.CCCD,
                    Email = command.CustomerInfo.Email,
                    Note = command.CustomerInfo.Note
                } : null,
                PaymentMethod = command.PaymentMethod,
                CreatedAt = DateTime.UtcNow
            };

            // Trả về OrderId và OrderCode cho frontend
            var result = new
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode
            };

            return Ok(BaseResponse<object>.Success(result));
        }

        /// <summary>
        /// Sinh mã OrderCode ngẫu nhiên (hoặc theo logic riêng)
        /// </summary>
        private string GenerateOrderCode()
        {
            // Tạo OrderCode ngẫu nhiên (có thể thay đổi logic này tùy theo nhu cầu)
            return $"ORD-{Guid.NewGuid().ToString().Substring(0, 8)}"; // Ví dụ: ORD-12345678
        }
    }

    // Lớp command nhận đầu vào cho API tạo đơn hàng
    public class CreateOrderCommand
    {
        public Guid MerchantBranchId { get; set; }
        public List<OrderItemCommand> OrderItems { get; set; }
        public bool IsInvoiceRequired { get; set; }
        public CustomerInfoCommand CustomerInfo { get; set; }
        public string PaymentMethod { get; set; }
    }

    // Lớp để truyền thông tin sản phẩm trong đơn hàng
    public class OrderItemCommand
    {
        public Guid ProductId { get; set; }
        public decimal ProductSellingPrice { get; set; }
        public decimal Quantities { get; set; }
        public decimal TotalPrice { get; set; }
    }

    // Lớp thông tin khách hàng (dành cho hóa đơn)
    public class CustomerInfoCommand
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string CCCD { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }

    // Lớp phản hồi thông tin đơn hàng
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public Guid MerchantBranchId { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public bool IsInvoiceRequired { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Lớp phản hồi cho các mục trong đơn hàng
    public class OrderItemResponse
    {
        public Guid ProductId { get; set; }
        public decimal ProductSellingPrice { get; set; }
        public decimal Quantities { get; set; }
        public decimal TotalPrice { get; set; }
    }

    // Lớp phản hồi thông tin khách hàng
    public class CustomerInfo
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string CCCD { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
}
