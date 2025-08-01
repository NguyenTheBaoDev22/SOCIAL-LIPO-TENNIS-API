using Applications.Features.MerchantBranches.Dtos;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.MerchantBranches.Queries
{
    /// <summary>
    /// Query để lấy thông tin MerchantBranch theo Mã số thuế (TaxNumber).
    /// </summary>
    public class GetMerchantBranchByTaxNumberQuery : IRequest<BaseResponse<GetMerchantBranchByTaxNumberRes>>
    {
        /// <summary>
        /// Mã số thuế của chi nhánh (10 hoặc 13 chữ số).
        /// </summary>
        public string TaxNumber { get; set; }
    }
}
