using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Merchants.Commands
{
    public class UpdateMerchantBranchStatusCallbackCommand
    {
        public Guid MerchantId { get; set; }
        public string MerchantCode { get; set; }
        public Guid MerchantBranchId { get; set; }
        public string MerchantBranchCode { get; set; }
        public string Status { get; set; } //EBranchStatus: TemporarilyClosed,Inactive,Active,PermanentlyClosed 
    }
}
