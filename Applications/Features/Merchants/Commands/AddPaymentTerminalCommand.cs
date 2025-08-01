using Applications.Features.Merchants.DTOs;
using Core.Enumerables;
using MediatR;
using Shared.Results;
using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Merchants.Commands
{
    /// <summary>
    /// Command để khai báo thiết bị thanh toán (PaymentTerminal) cho chi nhánh đã tồn tại.
    /// </summary>
    public class AddPaymentTerminalCommand : IRequest<BaseResponse<AddPaymentTerminalRes>>
    {
        /// <summary>
        /// Id của merchant
        /// </summary>
       // [Required(ErrorMessage = "MerchantId is required.")]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Mã merchant để xác thực trùng khớp với Id
        /// </summary>
       // [Required(ErrorMessage = "MerchantCode is required.")]
        public string MerchantCode { get; set; }

        /// <summary>
        /// Id của chi nhánh
        /// </summary>
       // [Required(ErrorMessage = "MerchantBranchId is required.")]
        public Guid MerchantBranchId { get; set; }

        /// <summary>
        /// Mã chi nhánh để xác thực trùng khớp với Id
        /// </summary>
      //  [Required(ErrorMessage = "MerchantBranchCode is required.")]
        public string MerchantBranchCode { get; set; }

        /// <summary>
        /// Tên thiết bị, ví dụ: Máy POS Quầy 1
        /// </summary>
       // [Required(ErrorMessage = "TerminalName is required.")]
        public string TerminalName { get; set; }

        /// <summary>
        /// Loại thiết bị: Soundbox, POS, Tablet, v.v.
        /// </summary>
        public string DeviceType { get; set; } = DeviceTypeConstants.Soundbox;

        /// <summary>
        /// Số serial của thiết bị, không được trùng trong hệ thống
        /// </summary>
        public string SerialNumber { get; set; }
       // public string? ActiveCallbackUrl { get; set; }
        // ===== Optional fields for future extension =====
        // public string IMEI { get; set; }
        // public string Manufacturer { get; set; }
        // public string Model { get; set; }
        // public string FirmwareVersion { get; set; }
        // public string DeviceId { get; set; }
        // public DateTime? LastSyncDate { get; set; }
        // public string ConfigurationJson { get; set; }
    }

}
