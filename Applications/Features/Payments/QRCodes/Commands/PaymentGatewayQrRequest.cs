using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Payments.QRCodes.Commands
{
    public class PaymentGatewayQrRequest
    {
        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string MerchantCode { get; set; }// NPHDMZP
        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string MerchantBranchCode { get; set; }//ZTT
        [Required]
        [MaxLength(2)]
        [MinLength(2)]
        public string PaymentTerminal { get; set; }//01
        [Required]
        public string QrType { get; set; } = "QRIBFTTA";

        [Range(2100, 499999999, ErrorMessage = "Số tiền phải nằm trong khoảng từ 2.100 đến 499.999.999")]
        public int Amount { get; set; }
        public string? PurposeOfTransaction { get; set; }

        public string? OrderCode { get; set; }
        [Required]
        public string Ipn { get; set; }


    }
}
