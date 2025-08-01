using Applications.Features.Merchants.DTOs;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Merchants.Queries
{

    public class LookupTaxCodeQuery : IRequest<BaseResponse<TaxCodeLookupResponse>>
    {
        public string TaxCode { get; }

        public LookupTaxCodeQuery(string taxCode)
        {
            TaxCode = taxCode;
        }
    }
}
