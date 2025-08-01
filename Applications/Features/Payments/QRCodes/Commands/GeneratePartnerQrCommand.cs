using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Payments.QRCodes.Commands
{
    public class GeneratePartnerQrCommand : IRequest<BaseResponse<string>>
    {
        [Required, MaxLength(3)] public string MerchantCode { get; set; } = default!;
        [Required, MaxLength(3)] public string MerchantBranchCode { get; set; } = default!;
        [Required, MaxLength(2)] public string PaymentTerminal { get; set; } = default!;
        [Required] public string QrType { get; set; } = "QRIBFTTA";
        [Range(2100, 499999999)] public int Amount { get; set; }
        public string? PurposeOfTransaction { get; set; }
        public string? OrderCode { get; set; }

        [Required] public string Ipn { get; set; } = default!;
    }

    public class GenerateQrCommand : IRequest<BaseResponse<string>>
    {
        public Guid MerchantBranchId { get; set; }
        [Range(2100, 499999999)] public int Amount { get; set; }
        public string? OrderCode { get; set; }
    }
}
